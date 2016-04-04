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
public class MyService {
    private static MyService ourInstance = new MyService();
    private Context context;
    private Class<?> serviceClass;
    private boolean bound;
    private IBinder binder;
    private Intent serviceIntent;
    private LocalServiceConnection serviceConnection;

    public static MyService getInstance() {
        return ourInstance;
    }

    private MyService() {
        serviceClass = TaskManager.class;
        context = ApplicationContext.getContext();
        serviceIntent = new Intent(context, serviceClass);
        serviceConnection = new LocalServiceConnection();
        assureServiceIsRunning();
        bound = false;
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
        if (!isBound()) {
            boolean cont = false;
            while (!cont) {
                cont = context.bindService(serviceIntent, serviceConnection, Context.BIND_ABOVE_CLIENT);
                if (!cont) {
                    try {
                        Thread.sleep(5000);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
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
