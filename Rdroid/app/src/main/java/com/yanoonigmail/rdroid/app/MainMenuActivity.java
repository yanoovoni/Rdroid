package com.yanoonigmail.rdroid.app;

import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;

import com.yanoonigmail.rdroid.R;

public class MainMenuActivity extends ActionBarActivity {
    private MyService mMyService = MyService.getInstance();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main_menu);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        mMyService.unbindService();
    }
}
