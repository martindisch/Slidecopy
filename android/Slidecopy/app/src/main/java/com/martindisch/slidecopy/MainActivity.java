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
import android.widget.EditText;
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

    static final int REQUEST_IMAGE_CAPTURE = 1;
    private EditText mCode;
    private TextView mStatus;
    private ProgressBar mProgress;
    private File mPhotoFile;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Button mPhoto = (Button) findViewById(R.id.bPhoto);
        mPhoto.setOnClickListener(this);
        mCode = (EditText) findViewById(R.id.etCode);
        mStatus = (TextView) findViewById(R.id.tvStatus);
        mProgress = (ProgressBar) findViewById(R.id.pbUpload);

        SharedPreferences prefs = getSharedPreferences("slidecopy", MODE_PRIVATE);
        String code = prefs.getString("code", "none");
        if (code.contentEquals("none")) {
            AsyncHttpClient client = new AsyncHttpClient();
            client.setMaxRetriesAndTimeout(0, 0);
            client.get(getString(R.string.code_url), new AsyncHttpResponseHandler() {
                @Override
                public void onSuccess(int statusCode, Header[] headers, byte[] responseBody) {
                    String newCode = new String(responseBody);
                    mCode.setText(newCode);
                    storeCode();
                }

                @Override
                public void onFailure(int statusCode, Header[] headers, byte[] responseBody, Throwable error) {
                    Log.e("CodeGen", "Couldn't reach server for code");
                }
            });
        } else {
            mCode.setText(code);
        }
    }

    @Override
    public void onClick(View view) {
        if (mCode.getText().length() > 0) {
            storeCode();
            Intent takePictureIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
            if (takePictureIntent.resolveActivity(getPackageManager()) != null) {
                mPhotoFile = new File(getExternalFilesDir(null), mCode.getText().toString() + ".jpg");
                Uri photoURI = FileProvider.getUriForFile(this,
                        "com.martindisch.fileprovider",
                        mPhotoFile);
                takePictureIntent.putExtra(MediaStore.EXTRA_OUTPUT, photoURI);
                startActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE);
            } else {
                Toast.makeText(this, R.string.no_camera, Toast.LENGTH_SHORT).show();
            }
        } else {
            Toast.makeText(this, R.string.enter_number, Toast.LENGTH_SHORT).show();
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

            mProgress.setProgress(0);
            mProgress.setVisibility(View.VISIBLE);
            mStatus.setText(R.string.uploading);
            mStatus.setVisibility(View.VISIBLE);

            AsyncHttpClient client = new AsyncHttpClient();
            client.setMaxRetriesAndTimeout(3, 500);
            client.post(getString(R.string.upload_url), params, new AsyncHttpResponseHandler() {

                @Override
                public void onProgress(long bytesWritten, long totalSize) {
                    Long lProgress = 100 * bytesWritten / totalSize;
                    int progress = lProgress.intValue();
                    if (progress != mProgress.getProgress()) {
                        mProgress.setProgress(progress);
                    }
                }

                @Override
                public void onSuccess(int statusCode, Header[] headers, byte[] bytes) {
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
                    mProgress.setVisibility(View.INVISIBLE);
                    mStatus.setText(R.string.failure);
                    mPhotoFile.delete();
                }
            });
        }
    }

    private void storeCode() {
        SharedPreferences prefs = getSharedPreferences("slidecopy", MODE_PRIVATE);
        if (!prefs.getString("code", "none").contentEquals(mCode.getText().toString())) {
            SharedPreferences.Editor editor = prefs.edit();
            editor.putString("code", mCode.getText().toString());
            editor.apply();
        }
    }
}
