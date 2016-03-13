package com.yanoonigmail.rdroid.app;

import android.app.ActivityManager;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.IBinder;

import com.yanoonigmail.rdroid.service.TaskManager;

/**
 * Created by 34v7 on 02/03/2016.
 */
public class Service implements ServiceConnection {
    private static Service ourInstance = new Service();

    public static Service getInstance() {
        return ourInstance;
    }
    private Context context = ApplicationContext.getContext();
    private Class<?> serviceClass = TaskManager.class;
    private boolean bound = false;
    private IBinder binder;
    private Intent serviceIntent;

    private Service() {
        serviceIntent = new Intent(context, serviceClass);

    }

    public boolean isRunning() {
        ActivityManager manager = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        for (ActivityManager.RunningServiceInfo service : manager.getRunningServices(Integer.MAX_VALUE)) {
            if (serviceClass.getName().equals(service.service.getClassName())) {
                return true;
            }
        }
        return false;
    }

    public boolean isBound() {
        return bound;
    }

    public void setBound(boolean bound) {
        this.bound = bound;
    }

    public IBinder getBinder() {
        return binder;
    }

    public void setBinder(IBinder binder) {
        this.binder = binder;
    }

    @Override
    public void onServiceConnected(ComponentName name, IBinder service) {
        setBound(true);
    }

    @Override
    public void onServiceDisconnected(ComponentName name) {
        setBound(false);
        boolean cont = false;
        while (!cont) {
            if (!isRunning()) {
                context.startService(serviceIntent);
            }
            if (context.bindService(serviceIntent, ourInstance, 0)) {
                cont = true;
            }
        }
    }
}
