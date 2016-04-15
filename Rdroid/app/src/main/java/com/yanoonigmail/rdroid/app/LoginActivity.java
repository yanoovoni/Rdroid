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
import android.widget.ProgressBar;
import android.widget.TextView;
import android.os.Parcel;

import com.yanoonigmail.rdroid.ApplicationContext;
import com.yanoonigmail.rdroid.service.TaskManager;

import java.lang.Thread;

import static com.yanoonigmail.rdroid.R.id.email;
import static com.yanoonigmail.rdroid.R.id.login_button;
import static com.yanoonigmail.rdroid.R.id.marker_progress1;
import static com.yanoonigmail.rdroid.R.id.marker_progress2;
import static com.yanoonigmail.rdroid.R.id.marker_progress3;
import static com.yanoonigmail.rdroid.R.id.password;
import static com.yanoonigmail.rdroid.R.id.status_text;
import static com.yanoonigmail.rdroid.R.layout.activity_login;
import static com.yanoonigmail.rdroid.R.string.status_login_attempt;
import static com.yanoonigmail.rdroid.R.string.status_login_bad_parameters;
import static com.yanoonigmail.rdroid.R.string.status_login_not_connected;
import static com.yanoonigmail.rdroid.R.string.status_login_wrong_parameters;
import static com.yanoonigmail.rdroid.R.string.user_data;
import static com.yanoonigmail.rdroid.service.TaskManager.IS_CONNECTED;
import static com.yanoonigmail.rdroid.service.TaskManager.TRY_LOGIN;

public class LoginActivity extends ActionBarActivity {
    private EditText mEmailEditText;
    private EditText mPasswordEditText;
    private Button mLoginButton;
    private TextView mStatusText;
    private ProgressBar mProgressBar1;
    private ProgressBar mProgressBar2;
    private ProgressBar mProgressBar3;
    private Thread mTryLoginThread;
    private MyService mMyService = MyService.getInstance();
    private Context mApplicationContext = ApplicationContext.getContext();
    private LoginActivity mLoginActivity = this;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(activity_login);
        mEmailEditText = (EditText) findViewById(email);
        mPasswordEditText = (EditText) findViewById(password);
        mLoginButton = (Button) findViewById(login_button);
        mStatusText = (TextView) findViewById(status_text);
        mProgressBar1 = (ProgressBar) findViewById(marker_progress1);
        mProgressBar2 = (ProgressBar) findViewById(marker_progress2);
        mProgressBar3 = (ProgressBar) findViewById(marker_progress3);
    }

    @Override
    protected void onStart() {
        super.onStart();
        mMyService.assureServiceIsRunning();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
    }

    public void tryLogin(View v) {
        setLoginInProgress(true);
        Parcel input_parcel = Parcel.obtain();
        Parcel output_parcel = Parcel.obtain();
        try {
            mMyService.getBinder().transact(IS_CONNECTED, input_parcel, output_parcel, 0);
            boolean[] val = new boolean[1];
            output_parcel.readBooleanArray(val);
            output_parcel.recycle();
            if (!val[0]) {
                mStatusText.setText(mApplicationContext.getString(status_login_not_connected));
                setLoginInProgress(false);
            } else {
                mStatusText.setText(mApplicationContext.getString(status_login_attempt));
                String given_email = mEmailEditText.getText().toString();
                String given_password = mPasswordEditText.getText().toString();
                if (!isEmailValid(given_email) || !isPasswordValid(given_password)) {
                    mStatusText.setText(mApplicationContext.getString(status_login_bad_parameters));
                    setLoginInProgress(false);
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
            setLoginInProgress(false);
            output_parcel.recycle();
        }
    }


    private boolean isEmailValid(String email) {
        return email.contains("@");
    }

    private boolean isPasswordValid(String password) {
        return (password.length() >= 1);
    }

    protected void setLoginInProgress(boolean inProgress) {
        mLoginButton.setEnabled(!inProgress);
        int visibility;
        if (inProgress) {
            visibility = View.VISIBLE;
        }
        else {
            visibility = View.INVISIBLE;
        }
        mProgressBar1.setVisibility(visibility);
        mProgressBar2.setVisibility(visibility);
        mProgressBar3.setVisibility(visibility);
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
            if (!mMyService.isRunning()) {
                throw new Error("MyService is ded");
            }
            Parcel input_parcel = Parcel.obtain();
            input_parcel.writeStringArray(strings);
            Parcel output_parcel = Parcel.obtain();
            boolean login_successful = false;
            try {
                mMyService.getBinder().transact(TRY_LOGIN, input_parcel, output_parcel, 0);
                boolean[] val = new boolean[1];
                output_parcel.readBooleanArray(val);
                login_successful = val[0];
                if (login_successful) {
                    SharedPreferences preferences = getSharedPreferences (getString(user_data), Context.MODE_PRIVATE);
                    SharedPreferences.Editor preferences_editor = preferences.edit();
                    preferences_editor.putString("email", strings[0]);
                    preferences_editor.putString("password", strings[1]);
                    preferences_editor.apply();
                }
            } catch (RemoteException e) {
                e.printStackTrace();
            }
            output_parcel.recycle();
            return login_successful;
        }

        @Override
        protected void onPostExecute(Boolean logged_in) {
            setLoginInProgress(false);
            if (logged_in) {
                Intent mainMenuIntent = new Intent(mLoginActivity, MainMenuActivity.class);
                mainMenuIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                mApplicationContext.startActivity(mainMenuIntent);
                finish();
            }
            else {
                mStatusText.setText(getString(status_login_wrong_parameters));
            }
        }
    }
}
