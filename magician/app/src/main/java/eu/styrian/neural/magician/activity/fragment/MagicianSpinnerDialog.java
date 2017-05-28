package eu.styrian.neural.magician.activity.fragment;

import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.os.Bundle;
import android.support.v4.app.DialogFragment;

/**
 * Created by Gal on 28/05/2017.
 */

public class MagicianSpinnerDialog extends DialogFragment {

    private ProgressDialog _dialog;

    public MagicianSpinnerDialog() {
        // use empty constructors. If something is needed use onCreate's
    }

    @Override
    public Dialog onCreateDialog(final Bundle savedInstanceState) {

        _dialog = new ProgressDialog(getActivity());
        this.setStyle(STYLE_NO_TITLE, getTheme()); // You can use styles or inflate a view
        _dialog.setTitle("Pošiljanje in obdelava slike"); // set your messages if not inflated from XML
        _dialog.setMessage("");
        _dialog.setCancelable(false);

        return _dialog;
    }

    public void setMessage(final String message) {
        Runnable changeMessage = new Runnable() {
            @Override
            public void run() {
                //Log.v(TAG, strCharacters);
                _dialog.setMessage(message);
            }
        };

        changeMessage.run();
    }
}