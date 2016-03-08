package com.yanoonigmail.rdroid.service;

        import android.content.BroadcastReceiver;
        import android.content.Context;
        import android.content.Intent;

/**
 * Created by yanoo on 24-Feb-16.
 */
public class StartupReceiver extends BroadcastReceiver {
    public StartupReceiver() {
    }

    @Override
    public void onReceive(Context context, Intent intent) {

        Intent myIntent = new Intent(context, TaskManager.class);
        context.startService(myIntent);

    }
}