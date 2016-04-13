package com.yanoonigmail.rdroid.app;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Parcel;
import android.os.RemoteException;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.View;

import com.yanoonigmail.rdroid.ApplicationContext;
import com.yanoonigmail.rdroid.R;

import static com.yanoonigmail.rdroid.service.TaskManager.LOGOUT;

public class MainMenuActivity extends ActionBarActivity {
    private MyService mMyService = MyService.getInstance();
    private Context mApplicationContext = ApplicationContext.getContext();
    private Activity mMainMenuActivity = this;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main_menu);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        mMyService.unbindService();
    }

    protected void logout(View view) {
        Parcel output_parcel = Parcel.obtain();
        Parcel input_parcel = Parcel.obtain();
        try {
            boolean[] logged_out = new boolean[1];
            mMyService.getBinder().transact(LOGOUT, input_parcel, output_parcel, 0);
            output_parcel.readBooleanArray(logged_out);
            if (logged_out[0]) {
                Intent loginIntent = new Intent(mMainMenuActivity, LoginActivity.class);
                loginIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                mApplicationContext.startActivity(loginIntent);
                finish();
            }
        } catch (RemoteException e) {
            e.printStackTrace();
        }
        output_parcel.recycle();
    }

    protected void closeApp(View view) {
        finish();
    }
}
