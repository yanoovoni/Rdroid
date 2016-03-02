package com.yanoonigmail.rdroid;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

/**
 * Created by yanoo on 24-Feb-16.
 */
public class BootUpReceiver extends BroadcastReceiver {
    public BootUpReceiver() {
        }

    @Override
    public void onReceive(Context context, Intent intent) {

        Intent myIntent = new Intent(context, TaskManager.class);
        context.startService(myIntent);

    }
}