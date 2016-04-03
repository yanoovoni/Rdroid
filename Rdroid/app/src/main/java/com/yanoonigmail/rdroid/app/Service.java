package com.yanoonigmail.rdroid.app;

import android.app.ActivityManager;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.IBinder;

import com.yanoonigmail.rdroid.ApplicationContext;
import com.yanoonigmail.rdroid.service.TaskManager;

/**
 * Created by 34v7 on 02/03/2016.
 */
public class Service {
    private static Service ourInstance = new Service();
    private Context context = ApplicationContext.getContext();
    private Class<TaskManager> serviceClass = TaskManager.class;
    private boolean bound = false;
    private IBinder binder;
    private Intent serviceIntent;
    private LocalServiceConnection serviceConnection;

    public static Service getInstance() {
        return ourInstance;
    }

    private Service() {
        serviceIntent = new Intent(context, serviceClass);
        serviceConnection = new LocalServiceConnection();
        assureServiceIsRunning();
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
        while (true) {
            if (isBound()) {
                return binder;
            } else {
                assureServiceIsRunning();
            }
        }
    }

    public void setBinder(IBinder binder) {
        this.binder = binder;
    }

    public void assureServiceIsRunning() {
        if (!isRunning()) {
            context.startService(serviceIntent);
        }
        boolean cont = true;
        while (cont) {
            cont = !context.bindService(serviceIntent, serviceConnection, Context.BIND_ABOVE_CLIENT);
            if (cont) {
                try {
                    Thread.sleep(5000);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
    }

    private class LocalServiceConnection implements ServiceConnection {
        @Override
        public void onServiceConnected(ComponentName name, IBinder service) {
            setBinder(service);
            setBound(true);
        }

        @Override
        public void onServiceDisconnected(ComponentName name) {
            setBound(false);
            assureServiceIsRunning();
        }
    }
}
