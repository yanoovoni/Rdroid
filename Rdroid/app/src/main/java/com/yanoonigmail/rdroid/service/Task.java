package com.yanoonigmail.rdroid.service;

import android.content.res.Resources;
import android.os.Environment;
import android.util.Log;

import com.yanoonigmail.rdroid.ApplicationContext;
import com.yanoonigmail.rdroid.R;

import java.io.File;

import static com.yanoonigmail.rdroid.R.string.protocol_client_task_results_output_separator;

/**
 * Created by yanoo on 19-Mar-16.
 */
public class Task {
    private String id;
    private String type;
    private String parameters;
    private Thread taskThread;
    private static Resources resources = ApplicationContext.getContext().getResources();

    public Task(String id, String type, String parameters) {
        this.id = id;
        this.type = type;
        this.parameters = parameters;
        initTask();
    }

    public String getId() {
        return id;
    }

    public String getType() {
        return type;
    }

    public Thread getTaskThread() {
        return taskThread;
    }

    private void initTask() {
        taskThread = new Thread(new Runnable() {
            public void run() {
                String output = "";
                switch (type) {
                    case "GET_FILES_IN_FOLDER":
                        output = getFilesInFolder(parameters.split(resources.getString(R.string.protocol_parameter_separator))[1]);
                }
                Server.getInstance().send(Protocol.taskResultsMessage(id, output));
            }
        });
        taskThread.start();
    }

    private String getFilesInFolder(String folder) {
        String filesString = "";
        File sdCardRoot = Environment.getExternalStorageDirectory();
        File yourDir = new File(sdCardRoot, folder);
        for (File f : yourDir.listFiles()) {
            String name;
            if (f.isFile()) {
                name = f.getName();
                Log.i("file name", name);
            }
            else {
                name = f.getName() + ":folder";
            }
            filesString += name + resources.getString(protocol_client_task_results_output_separator);
        }
        return filesString;
    }

}
