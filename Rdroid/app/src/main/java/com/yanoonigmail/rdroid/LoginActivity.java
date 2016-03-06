package com.yanoonigmail.rdroid;

import android.content.Intent;
import android.os.AsyncTask;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import java.lang.Thread;

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
    private Thread mTryLoginThread;

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

    @Override
    protected void onStart() {
        super.onStart();
        if (!GlobalInfo.getInstance().isServiceRunning()) {
            startService(new Intent(this, TaskManager.class));
        }
    }

    public void tryLogin(View v) {
        mLoginButton.setEnabled(false);
        if (!mServer.isConnected()) {
            mStatusText.setText(getString(status_login_not_connected));
        } else {
            mStatusText.setText(getString(status_login_attempt));
            String given_email = mEmailEditText.getText().toString();
            String given_password = mPasswordEditText.getText().toString();
            if (!isEmailValid(given_email) || !isPasswordValid(given_password)) {
                mStatusText.setText(getString(status_login_bad_parameters));
                mLoginButton.setEnabled(true);
            } else {
                new AsyncLogin().execute(given_email, given_password);
            }
        }
    }

    private boolean isEmailValid(String email) {
        return email.contains("@");
    }

    private boolean isPasswordValid(String password) {
        return (password.length() >= 4);
    }

    private class AsyncLogin extends AsyncTask<String, Void, Boolean> {
        @Override
        protected Boolean doInBackground(String... strings) {
            if (strings.length != 2) {
                throw new IllegalArgumentException("Method requires exactly 2 strings");
            }
            String email = strings[0];
            String password = strings[1];
            return mServer.tryLogin(email, password);
        }

        @Override
        protected void onPostExecute(Boolean logged_in) {
            mLoginButton.setEnabled(true);
            if (logged_in) {
                Intent i = new Intent(ApplicationContext.getContext(), MainMenuActivity.class);
                ApplicationContext.getContext().startActivity(i);
            }
            else {
                mStatusText.setText(getString(status_login_wrong_parameters));
            }
        }

    }
}
