package com.yanoonigmail.rdroid;

import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;

public class LoginActivity extends ActionBarActivity {
    private Server mServer;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        mServer = Server.getInstance();
        mServer.connect();
    }
}
