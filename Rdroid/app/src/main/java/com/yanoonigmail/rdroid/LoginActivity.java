package com.yanoonigmail.rdroid;

import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.widget.EditText;

public class LoginActivity extends ActionBarActivity {
    private Server mServer;
    private EditText mEditText;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        mEditText = (EditText) findViewById(R.id.email);
        mServer = Server.getInstance();

    }
}
