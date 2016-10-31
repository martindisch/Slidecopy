package com.martindisch.slidecopy;

import android.content.Intent;
import android.net.Uri;
import android.os.Environment;
import android.provider.MediaStore;
import android.support.v4.content.FileProvider;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import java.io.File;
import java.io.IOException;

public class MainActivity extends AppCompatActivity implements View.OnClickListener {

    static final int REQUEST_IMAGE_CAPTURE = 1;
    private Button mPhoto;
    private EditText mCode;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        mPhoto = (Button) findViewById(R.id.bPhoto);
        mPhoto.setOnClickListener(this);
        mCode = (EditText) findViewById(R.id.etCode);
    }

    @Override
    public void onClick(View view) {
        if (mCode.getText().length() > 0) {
            Intent takePictureIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
            if (takePictureIntent.resolveActivity(getPackageManager()) != null) {
                File photoFile = null;
                try {
                    photoFile = createImageFile(mCode.getText().toString());
                } catch (IOException e) {
                    e.printStackTrace();
                    Toast.makeText(this, R.string.no_file, Toast.LENGTH_SHORT).show();
                }
                if (photoFile != null) {
                    Uri photoURI = FileProvider.getUriForFile(this,
                            "com.martindisch.fileprovider",
                            photoFile);
                    takePictureIntent.putExtra(MediaStore.EXTRA_OUTPUT, photoURI);
                    startActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE);
                }
            } else {
                Toast.makeText(this, R.string.no_camera, Toast.LENGTH_SHORT).show();
            }
        } else {
            Toast.makeText(this, R.string.enter_number, Toast.LENGTH_SHORT).show();
        }
    }

    private File createImageFile(String code) throws IOException {
        return new File(getExternalFilesDir(null), code + ".jpg");
    }
}
