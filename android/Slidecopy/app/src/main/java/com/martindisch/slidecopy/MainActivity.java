package com.martindisch.slidecopy;

import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.provider.MediaStore;
import android.support.v4.content.FileProvider;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.loopj.android.http.AsyncHttpClient;
import com.loopj.android.http.AsyncHttpResponseHandler;
import com.loopj.android.http.RequestParams;

import java.io.File;
import java.io.FileNotFoundException;

import cz.msebera.android.httpclient.Header;

public class MainActivity extends AppCompatActivity implements View.OnClickListener {

    private static final int REQUEST_IMAGE_CAPTURE = 1;
    private TextView mCode;
    private TextView mStatus;
    private ProgressBar mProgress;
    private File mPhotoFile;
    private String code;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        code = null;

        // basic view initialization
        Button mPhoto = (Button) findViewById(R.id.bPhoto);
        mPhoto.setOnClickListener(this);
        mCode = (TextView) findViewById(R.id.tvCode);
        mStatus = (TextView) findViewById(R.id.tvStatus);
        mProgress = (ProgressBar) findViewById(R.id.pbUpload);

        // try to load saved code
        SharedPreferences prefs = getSharedPreferences("slidecopy", MODE_PRIVATE);
        String loadedCode = prefs.getString("code", "none");
        if (loadedCode.contentEquals("none")) {
            getCode(false);
        } else {
            code = loadedCode;
            mCode.setText(code);
        }
    }

    @Override
    public void onClick(View view) {
        if (code != null) {
            takePicture();
        } else {
            getCode(true);
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == REQUEST_IMAGE_CAPTURE && resultCode == RESULT_OK) {
            // gather your request parameters
            RequestParams params = new RequestParams();
            try {
                params.put("file", mPhotoFile);
            } catch (FileNotFoundException e) {
                e.printStackTrace();
                Toast.makeText(getApplicationContext(), R.string.oh_no, Toast.LENGTH_SHORT).show();
            }

            // initialize views for upload
            mProgress.setProgress(0);
            mProgress.setVisibility(View.VISIBLE);
            mStatus.setText(R.string.uploading);
            mStatus.setVisibility(View.VISIBLE);

            // start uploading
            AsyncHttpClient client = new AsyncHttpClient();
            client.setMaxRetriesAndTimeout(3, 500);
            client.post(getString(R.string.upload_url), params, new AsyncHttpResponseHandler() {

                @Override
                public void onProgress(long bytesWritten, long totalSize) {
                    // update progress bar
                    Long lProgress = 100 * bytesWritten / totalSize;
                    int progress = lProgress.intValue();
                    if (progress != mProgress.getProgress()) {
                        mProgress.setProgress(progress);
                    }
                }

                @Override
                public void onSuccess(int statusCode, Header[] headers, byte[] bytes) {
                    // hide progressBar, inform user of success and delete photo
                    mProgress.setVisibility(View.INVISIBLE);
                    mStatus.setText(R.string.success);
                    mPhotoFile.delete();
                }

                @Override
                public void onRetry(int retryNo) {
                    mStatus.setText(getString(R.string.retrying, retryNo));
                }

                @Override
                public void onFailure(int statusCode, Header[] headers, byte[] bytes, Throwable throwable) {
                    // hide progressBar, inform user of failure and delete photo
                    mProgress.setVisibility(View.INVISIBLE);
                    mStatus.setText(R.string.failure);
                    mPhotoFile.delete();
                }
            });
        }
    }

    /**
     * Starts an Intent to take a photo using a preinstalled camera app and saving the picture
     * in the apps private directory with the code as filename.
     */
    private void takePicture() {
        Intent takePictureIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        // check if there is a camera app installed
        if (takePictureIntent.resolveActivity(getPackageManager()) != null) {
            // build the photo file
            mPhotoFile = new File(getExternalFilesDir(null), code + ".jpg");
            // get URI using a file provider
            Uri photoURI = FileProvider.getUriForFile(this,
                    "com.martindisch.fileprovider",
                    mPhotoFile);
            // put URI into Intent
            takePictureIntent.putExtra(MediaStore.EXTRA_OUTPUT, photoURI);
            // send Intent
            startActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE);
        } else {
            Toast.makeText(this, R.string.no_camera, Toast.LENGTH_SHORT).show();
        }
    }

    /**
     * Contacts the server to get a unique code. Save and display the code on success, inform
     * user on failure.
     *
     * @param takePicture   Whether or not to take a picture after a code has been received
     */
    private void getCode(final boolean takePicture) {
        Toast.makeText(this, R.string.contacting_server, Toast.LENGTH_SHORT).show();
        // make GET request
        AsyncHttpClient client = new AsyncHttpClient();
        client.setMaxRetriesAndTimeout(0, 0);
        client.get(getString(R.string.code_url), new AsyncHttpResponseHandler() {
            @Override
            public void onSuccess(int statusCode, Header[] headers, byte[] responseBody) {
                code = new String(responseBody);
                // display new code
                mCode.setText(code);
                // store the code
                storeCode(code);
                Toast.makeText(getApplicationContext(), R.string.code_received, Toast.LENGTH_SHORT).show();
                // if user got here by pressing 'take picture', take the picture
                if (takePicture) takePicture();
            }

            @Override
            public void onFailure(int statusCode, Header[] headers, byte[] responseBody, Throwable error) {
                // inform user of failure
                mCode.setText(R.string.no_code);
                Toast.makeText(getApplicationContext(), R.string.no_code_explanation, Toast.LENGTH_SHORT).show();
            }
        });
    }

    /**
     * Stores the code in SharedPreferences.
     *
     * @param code  the code to store
     */
    private void storeCode(String code) {
        SharedPreferences prefs = getSharedPreferences("slidecopy", MODE_PRIVATE);
            SharedPreferences.Editor editor = prefs.edit();
            editor.putString("code", code);
            editor.apply();
    }
}
