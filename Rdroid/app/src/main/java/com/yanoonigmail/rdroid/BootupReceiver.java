package com.yanoonigmail.rdroid;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

/**
 * Created by yanoo on 24-Feb-16.
 */
public class BootupReceiver extends BroadcastReceiver {

    @Override
    public void onReceive(Context context, Intent intent) {

        Intent myIntent = new Intent(context, BootupService.class);
        context.startService(myIntent);

    }
}