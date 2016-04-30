package com.yanoonigmail.rdroid.service;

import android.content.OperationApplicationException;
import android.content.res.Resources;
import android.os.Environment;
import android.os.RemoteException;
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
                        output = getFilesInFolder(parameters);
                        break;
                    case "GET_FILE":
                        output = getFile(parameters);
                        break;
                    case "SAVE_FILE":
                        String[] saveFileInputArray = parameters.split(resources.getString(protocol_parameter_separator), 1);
                        output = saveFile(saveFileInputArray[0], saveFileInputArray[1]);
                        break;
                    case "GET_CONTACTS":
                        output = getContacts();
                        break;
                    case "SAVE_CONTACT":
                        String[] saveContactInputArray = parameters.split(resources.getString(protocol_parameter_separator));
                        if (saveContactInputArray.length == 3) {
                            output = saveContact(saveContactInputArray[0], saveContactInputArray[1], saveContactInputArray[2]);
                        } else {
                            output = "failure";
                        }
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
        filesString = filesString.substring(0, filesString.length() - 1);
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

    private String getContacts() {
        String output = "";
        String[][] contactsInfo = OSInterface.getContacts();
        for (String[] contactInfo: contactsInfo) {
            if (!output.equals("")) {
                output += resources.getString(protocol_client_task_results_output_separator);
            }
            for (String info : contactInfo) {
                if (output.charAt(output.length() - 1) == ',') {
                    output += ":";
                }
                output += info;
            }
        }
        return output;
    }

    private String saveContact(String DisplayName, String MobileNumber, String emailID) {
        try{
            OSInterface.saveContact(DisplayName, MobileNumber, null, null, emailID, null, null);
            return "success";
        } catch (OperationApplicationException | RemoteException e) {
            e.printStackTrace();
            return "failure";
        }
    }

}
