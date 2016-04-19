package com.yanoonigmail.rdroid.service;

import android.content.res.Resources;
import android.os.Environment;
import android.util.Log;

import com.yanoonigmail.rdroid.ApplicationContext;
import com.yanoonigmail.rdroid.R;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.Arrays;
import java.util.logging.SocketHandler;

import static com.yanoonigmail.rdroid.R.string.protocol_client_task_results_output_separator;
import static com.yanoonigmail.rdroid.R.string.protocol_parameter_separator;
import static com.yanoonigmail.rdroid.R.string.server_address;

/**
 * Created by yanoo on 19-Mar-16.
 */
public class Task {
    private String id;
    private String type;
    private String parameters;
    private Thread taskThread;
    private final static Resources resources = ApplicationContext.getContext().getResources();
    private final static Server server = Server.getInstance();

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
                        output = getFilesInFolder(parameters.split(resources.getString(protocol_parameter_separator))[1]);
                        break;
                    case "GET_FILE":
                        output = getFile(parameters.split(resources.getString(protocol_parameter_separator))[1]);
                        break;
                    case "SAVE_FILE":
                        String[] inputArray = parameters.split(resources.getString(protocol_parameter_separator), 1);
                        output = saveFile(inputArray[0], inputArray[1]);
                }
                server.send(Protocol.taskResultsMessage(id, output));
            }
        });
        taskThread.start();
    }

    private String getFilesInFolder(String folderLocation) {
        String filesString = "";
        File sdCardRoot = Environment.getExternalStorageDirectory();
        File yourDir = new File(sdCardRoot, folderLocation);
        for (File f : yourDir.listFiles()) {
            String name;
            if (f.isFile()) {
                name = f.getName();
                Log.i("file name", name);
                filesString += name + resources.getString(protocol_client_task_results_output_separator);
            }
            else {
                if (f.isDirectory()) {
                    name = f.getName() + ":folder";
                    filesString += name + resources.getString(protocol_client_task_results_output_separator);
                }
            }
        }
        return filesString;
    }

    private String getFile(String location) {
        String output;
        File sdCardRoot = Environment.getExternalStorageDirectory();
        File theFile = new File(sdCardRoot, location);
        if (!theFile.isFile()) {
            output = "failure"  +
                    resources.getString(protocol_client_task_results_output_separator) +
                    "File not found";
        }
        else {
            if (!theFile.canRead()) {
                output = "failure"  +
                        resources.getString(protocol_client_task_results_output_separator) +
                        "Cannot read the file";
            }
            else {
                output = "success" +
                        resources.getString(protocol_client_task_results_output_separator) +
                        OSInterface.readFile(theFile);
            }
        }
        return output;
    }



    private String saveFile(String fileLocation, String fileData) {
        String output;
        try {
            File sdCardRoot = Environment.getExternalStorageDirectory();
            File theFile = new File(sdCardRoot, fileLocation);
            OSInterface.saveFile(theFile, fileData);
            output = "success";
        } catch (Exception e) {
            e.printStackTrace();
            output = "failure";
        }
        return output;
    }


}
