package com.yanoonigmail.rdroid;

import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import static com.yanoonigmail.rdroid.R.id.email;
import static com.yanoonigmail.rdroid.R.id.login_button;
import static com.yanoonigmail.rdroid.R.id.password;
import static com.yanoonigmail.rdroid.R.id.status_text;
import static com.yanoonigmail.rdroid.R.layout.activity_login;
import static com.yanoonigmail.rdroid.R.string.status_login_attempt;
import static com.yanoonigmail.rdroid.R.string.status_login_bad_parameters;
import static com.yanoonigmail.rdroid.R.string.status_login_not_connected;
import static com.yanoonigmail.rdroid.R.string.status_login_successful;
import static com.yanoonigmail.rdroid.R.string.status_login_wrong_parameters;

public class LoginActivity extends ActionBarActivity {
    private Server mServer;
    private EditText mEmailEditText;
    private EditText mPasswordEditText;
    private Button mLoginButton;
    private TextView mStatusText;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(activity_login);
        mEmailEditText = (EditText) findViewById(email);
        mPasswordEditText = (EditText) findViewById(password);
        mLoginButton = (Button) findViewById(login_button);
        mStatusText = (TextView) findViewById(status_text);
        mServer = Server.getInstance();
    }

    public void tryLogin(View v) {
        if (!mServer.isConnected()) {
            mStatusText.setText(getString(status_login_not_connected));
        }
        else {
            mStatusText.setText(getString(status_login_attempt));
            String given_email = mEmailEditText.getText().toString();
            String given_password = mPasswordEditText.getText().toString();
            if (!isEmailValid(given_email) || !isPassswordVaild(given_password)) {
                mStatusText.setText(getString(status_login_bad_parameters));
            } else {
                boolean login_successful = mServer.tryLogin(given_email, given_password);
                if (login_successful) {
                    mStatusText.setText(getString(status_login_successful));
                } else {
                    mStatusText.setText(getString(status_login_wrong_parameters));
                }
            }
        }
    }

    private boolean isEmailValid(String email) {
        return email.contains("@");
    }

    private boolean isPassswordVaild(String password) {
        return (password.length() >= 4);
    }
}
