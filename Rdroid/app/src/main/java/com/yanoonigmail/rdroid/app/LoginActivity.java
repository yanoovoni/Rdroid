package com.yanoonigmail.rdroid.app;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.os.RemoteException;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.os.Parcel;

import com.yanoonigmail.rdroid.ApplicationContext;

import java.lang.Thread;

import static com.yanoonigmail.rdroid.R.id.email;
import static com.yanoonigmail.rdroid.R.id.login_button;
import static com.yanoonigmail.rdroid.R.id.password;
import static com.yanoonigmail.rdroid.R.id.status_text;
import static com.yanoonigmail.rdroid.R.layout.activity_login;
import static com.yanoonigmail.rdroid.R.string.status_login_attempt;
import static com.yanoonigmail.rdroid.R.string.status_login_bad_parameters;
import static com.yanoonigmail.rdroid.R.string.status_login_not_connected;
import static com.yanoonigmail.rdroid.R.string.status_login_wrong_parameters;
import static com.yanoonigmail.rdroid.R.string.user_data;

public class LoginActivity extends ActionBarActivity {
    private EditText mEmailEditText;
    private EditText mPasswordEditText;
    private Button mLoginButton;
    private TextView mStatusText;
    private Thread mTryLoginThread;
    private Service mService = Service.getInstance();
    private Context mApplicationContext = ApplicationContext.getContext();
    private LoginActivity mLoginActivity = this;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        SharedPreferences preferences = getSharedPreferences (getString(user_data), Context.MODE_PRIVATE);
        if (preferences.contains("email") && preferences.contains("password")) {
            Intent i = new Intent(mApplicationContext, MainMenuActivity.class);
            mApplicationContext.startActivity(i);
        }
        else {
            setContentView(activity_login);
            mEmailEditText = (EditText) findViewById(email);
            mPasswordEditText = (EditText) findViewById(password);
            mLoginButton = (Button) findViewById(login_button);
            mStatusText = (TextView) findViewById(status_text);
        }
    }

    @Override
    protected void onStart() {
        super.onStart();
        mService.assureServiceIsRunning();
    }

    public void tryLogin(View v) {
        mLoginButton.setEnabled(false);
        try {
            Parcel input_parcel = Parcel.obtain();
            Parcel output_parcel = Parcel.obtain();
            mService.getBinder().transact(2, input_parcel, output_parcel, 0);
            boolean[] val = new boolean[1];
            output_parcel.readBooleanArray(val);
            if (!val[0]) {
                mStatusText.setText(mApplicationContext.getString(status_login_not_connected));
                mLoginButton.setEnabled(true);
            } else {
                mStatusText.setText(mApplicationContext.getString(status_login_attempt));
                String given_email = mEmailEditText.getText().toString();
                String given_password = mPasswordEditText.getText().toString();
                if (!isEmailValid(given_email) || !isPasswordValid(given_password)) {
                    mStatusText.setText(mApplicationContext.getString(status_login_bad_parameters));
                    mLoginButton.setEnabled(true);
                } else {
                    try {
                        new AsyncLogin().execute(given_email, given_password);
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                }
            }
        } catch (RemoteException e) {
            e.printStackTrace();
            mLoginButton.setEnabled(true);
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
        /**
         String email = strings[0];
         String password = strings[1];
         * **/
        protected Boolean doInBackground(String... strings) {
            if (strings.length != 2) {
                throw new IllegalArgumentException("Method requires exactly 2 strings");
            }
            if (!mService.isRunning()) {
                throw new Error("Service is ded");
            }
            Parcel input_parcel = Parcel.obtain();
            input_parcel.writeStringArray(strings);
            Parcel output_parcel = Parcel.obtain();
            boolean login_result = false;
            try {
                mService.getBinder().transact(1, input_parcel, output_parcel, 0);
                boolean[] val = new boolean[1];
                output_parcel.readBooleanArray(val);
                login_result = val[0];
            } catch (RemoteException e) {
                e.printStackTrace();
            }
            return login_result;
        }

        @Override
        protected void onPostExecute(Boolean logged_in) {
            mLoginButton.setEnabled(true);
            if (logged_in) {
                Intent mainMenuIntent = new Intent(mLoginActivity, MainMenuActivity.class);
                mainMenuIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                mApplicationContext.startActivity(mainMenuIntent);
            }
            else {
                mStatusText.setText(getString(status_login_wrong_parameters));
            }
        }

    }
}
