package com.yanoonigmail.rdroid.app;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Parcel;
import android.os.RemoteException;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;

import com.yanoonigmail.rdroid.ApplicationContext;
import com.yanoonigmail.rdroid.R;
import com.yanoonigmail.rdroid.service.TaskManager;

import static com.yanoonigmail.rdroid.R.string.user_data;
import static com.yanoonigmail.rdroid.service.TaskManager.IS_LOGGED_IN;

public class LoadingActivity extends ActionBarActivity {
    private MyService mMyService = MyService.getInstance();
    private Context mApplicationContext = ApplicationContext.getContext();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_loading);
        load();
    }

    protected void load() {
        Thread loadingThread = new Thread(new Runnable() {
            public void run() {
                mMyService.assureServiceIsRunning();
                Parcel input_parcel = Parcel.obtain();
                Parcel output_parcel = Parcel.obtain();
                try {
                    mMyService.getBinder().transact(IS_LOGGED_IN, input_parcel, output_parcel, 0);
                } catch (RemoteException e) {
                    e.printStackTrace();
                }
                boolean[] bool_array = new boolean[1];
                output_parcel.readBooleanArray(bool_array);
                output_parcel.recycle();
                Intent i;
                if (bool_array[0]) {
                    i = new Intent(mApplicationContext, MainMenuActivity.class);
                } else {
                    i = new Intent(mApplicationContext, LoginActivity.class);
                }
                i.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                mApplicationContext.startActivity(i);
                finish();
            }
        });
        loadingThread.start();
    }
}
