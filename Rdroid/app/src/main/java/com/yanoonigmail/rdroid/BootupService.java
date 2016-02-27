package com.yanoonigmail.rdroid;

import android.app.Service;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.IBinder;
import android.content.Context;
import android.os.SystemClock;

import static com.yanoonigmail.rdroid.R.string.user_data;

/**
 * Created by Yaniv on 24-Feb-16.
 */
public class BootupService extends Service {
    private String mUsername;
    private String mPassword;

    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    public void onCreate() {
        super.onCreate();
        SharedPreferences preferences = getSharedPreferences (getString(user_data), Context.MODE_PRIVATE);
        while (!(preferences.contains("username") && preferences.contains("password")))
            SystemClock.sleep(1000);
        mUsername = preferences.getString("username", "");
        mPassword = preferences.getString("password", "");

    }

}
