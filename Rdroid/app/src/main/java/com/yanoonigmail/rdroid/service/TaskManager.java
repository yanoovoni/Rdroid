package com.yanoonigmail.rdroid.service;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.TaskStackBuilder;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.BitmapFactory;
import android.os.Binder;
import android.os.IBinder;
import android.content.Context;
import android.os.Parcel;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

import com.yanoonigmail.rdroid.ApplicationContext;
import com.yanoonigmail.rdroid.R;
import com.yanoonigmail.rdroid.app.LoadingActivity;

import java.util.ArrayList;
import java.util.Random;

import static com.yanoonigmail.rdroid.R.string.user_data;

/**
 * Created by Yaniv on 24-Feb-16.
 */
public class TaskManager extends android.app.Service {
    public static final int TRY_LOGIN = 1;
    public static final int IS_CONNECTED = 2;
    public static final int IS_LOGGED_IN = 3;
    public static final int LOGOUT = 4;
    private Server mServer;
    private Thread mManageTasksThread;
    private ArrayList<String> mIDList = new ArrayList<>();
    // Binder given to clients
    private final LocalBinder mBinder = new LocalBinder();
    // Random number generator
    private final Random mGenerator = new Random();
    private int mId = 9000;

    @Override
    public IBinder onBind(Intent intent) {
        return mBinder;
    }

    @Override
    public void onCreate() {
        super.onCreate();
        Log.d("service create", "service is alive");
        setForeground();
        mServer = Server.getInstance();
        manageTasks();
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        super.onStartCommand(intent, flags, startId);
        Log.d("service start", "service is alive");
        mServer = Server.getInstance();
        manageTasks();
        return START_NOT_STICKY;
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
    }

    public class LocalBinder extends Binder {
        TaskManager getService() {
            // Return this instance of the service so clients can call public methods.
            return TaskManager.this;
        }
        @Override
        protected boolean onTransact(int code, Parcel data, Parcel reply,
                                     int flags) {
            boolean success = false;
            switch (code) {
                case TRY_LOGIN:
                    String[] inputStrings = new String[2];
                    data.readStringArray(inputStrings);
                    boolean[] outputBoolean = new boolean[1];
                    outputBoolean[0] = mServer.tryLogin(inputStrings[0], inputStrings[1]);
                    reply.writeBooleanArray(outputBoolean);
                    success = true;
                    break;
                case IS_CONNECTED:
                    boolean[] connectedBool = new boolean[1];
                    connectedBool[0] = mServer.isConnected();
                    reply.writeBooleanArray(connectedBool);
                    success = true;
                    break;
                case IS_LOGGED_IN:
                    boolean[] loggedInBool = new boolean[1];
                    loggedInBool[0] = mServer.isLoggedInAfterFirstTry();
                    reply.writeBooleanArray(loggedInBool);
                    success = true;
                    break;
                case LOGOUT:
                    boolean disconnected = mServer.disconnect();
                    if (disconnected) {
                        SharedPreferences preferences = getSharedPreferences(getString(user_data), Context.MODE_PRIVATE);
                        SharedPreferences.Editor preferences_editor = preferences.edit();
                        preferences_editor.remove("email");
                        preferences_editor.remove("password");
                        preferences_editor.apply();
                    }
                    boolean[] output_bool_array = new boolean[1];
                    output_bool_array[0] = disconnected;
                    reply.writeBooleanArray(output_bool_array);
            }
            data.recycle();
            return success;
        }
    }

    private void manageTasks() {
        if (mManageTasksThread != null) {
            if (!mManageTasksThread.isAlive()) {
                mManageTasksThread = new Thread(new Runnable() {
                    public void run() {
                        while (true) {
                            if (mServer.isLoggedIn()) {
                                try {
                                    Task task = Protocol.taskRequest(mServer.recv());
                                    mIDList.add(task.getId());
                                } catch (Exception e) {
                                    e.printStackTrace();
                                }
                            } else {
                                try {
                                    Thread.sleep(1000);
                                } catch (InterruptedException e) {
                                    e.printStackTrace();
                                }
                            }
                        }
                    }
                });
                mManageTasksThread.start();
            }
        }
    }

    private void notifyUser() {
        Notification mNotification =
                new Notification.Builder(ApplicationContext.getContext())
                        .setContentTitle("RDroid is running")
                        .setContentText("The RDroid service is running.")
                        .setSmallIcon(R.mipmap.ic_launcher)
                        .setLargeIcon(BitmapFactory.decodeResource(getResources(), R.mipmap.ic_launcher))
                        .build();
        // Creates an explicit intent for an Activity in your app
        Intent resultIntent = new Intent(this, LoadingActivity.class);

        // The stack builder object will contain an artificial back stack for the
        // started Activity.
        // This ensures that navigating backward from the Activity leads out of
        // your application to the Home screen.
        TaskStackBuilder stackBuilder = TaskStackBuilder.create(this);
        // Adds the back stack for the Intent (but not the Intent itself)
        stackBuilder.addParentStack(LoadingActivity.class);
        // Adds the Intent that starts the Activity to the top of the stack
        stackBuilder.addNextIntent(resultIntent);
        NotificationManager mNotificationManager =
                (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
        // mId allows you to update the notification later on.
        mNotificationManager.notify(mId, mNotification);
    }

    private void setForeground() {
        Notification mNotification =
                new Notification.Builder(ApplicationContext.getContext())
                        .setContentTitle("RDroid is running")
                        .setContentText("The RDroid service is running")
                        .setSmallIcon(R.mipmap.ic_launcher)
                        .setLargeIcon(BitmapFactory.decodeResource(getResources(), R.mipmap.ic_launcher))
                        .setOngoing(true)
                        .build();
        startForeground(mId, mNotification);
    }
}



