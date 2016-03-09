package com.yanoonigmail.rdroid.app;

import android.app.Service;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.os.Binder;
import android.os.IBinder;
import android.os.IInterface;
import android.os.RemoteException;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.os.Parcel;

import com.yanoonigmail.rdroid.service.TaskManager;

import java.io.FileDescriptor;
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
    private Intent mServiceIntent;
    private ServiceConnection mServiceConnection;
    private IBinder mServiceBinder;
    private boolean mServiceConnected = false;
    private Thread mTryLoginThread;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(activity_login);
        mEmailEditText = (EditText) findViewById(email);
        mPasswordEditText = (EditText) findViewById(password);
        mLoginButton = (Button) findViewById(login_button);
        mStatusText = (TextView) findViewById(status_text);
        SharedPreferences preferences = getSharedPreferences (getString(user_data), Context.MODE_PRIVATE);
        if (preferences.contains("email") && preferences.contains("password")) {
            Intent i = new Intent(ApplicationContext.getContext(), MainMenuActivity.class);
            ApplicationContext.getContext().startActivity(i);
        }
    }

    @Override
    protected void onStart() {
        super.onStart();
        if (!GlobalInfo.getInstance().isServiceRunning()) {
            mServiceIntent = new Intent(this, TaskManager.class);
            mServiceConnection = new ServiceConnection() {
                @Override
                public void onServiceConnected(ComponentName name, IBinder service) {
                    mServiceConnected = true;
                    mServiceBinder = service;
                }

                @Override
                public void onServiceDisconnected(ComponentName name) {
                    mServiceConnected = false;
                }
            };
            startService(mServiceIntent);
            while (!bindService(mServiceIntent, mServiceConnection, 0)) {
                try {
                    wait(5000);
                }
                catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
    }

    public void tryLogin(View v) {
        mLoginButton.setEnabled(false);
        if (!mServiceConnected) {
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
        /**
         String email = strings[0];
         String password = strings[1];
         * **/
        protected Boolean doInBackground(String... strings) {
            if (strings.length != 2) {
                throw new IllegalArgumentException("Method requires exactly 2 strings");
            }
            Parcel input_parcel = Parcel.obtain();
            input_parcel.writeStringArray(strings);
            Parcel output_parcel = Parcel.obtain();
            boolean login_result = false;
            try {
                mServiceBinder.transact(IBinder.FIRST_CALL_TRANSACTION, input_parcel, output_parcel, 0);
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
                Intent i = new Intent(ApplicationContext.getContext(), MainMenuActivity.class);
                ApplicationContext.getContext().startActivity(i);
            }
            else {
                mStatusText.setText(getString(status_login_wrong_parameters));
            }
        }

    }
}
