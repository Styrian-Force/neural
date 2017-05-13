package eu.styrian.neural.magician;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.DividerItemDecoration;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.util.Log;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.List;

import butterknife.ButterKnife;
import eu.styrian.neural.magician.api.adapters.AnswersAdapter;
import eu.styrian.neural.magician.api.adapters.ValueAdapter;
import eu.styrian.neural.magician.api.interfaces.ValueService;
import eu.styrian.neural.magician.api.models.Item;
import eu.styrian.neural.magician.api.models.SOAnswersResponse;
import eu.styrian.neural.magician.api.models.Value;
import eu.styrian.neural.magician.api.utils.ApiUtils;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;


public class MainActivity extends AppCompatActivity {

    /*@BindView(R.id.textview)
    TextView textView;

    @BindView(R.id.button)
    Button buttonSend;*/

    private ValueAdapter valueAdapter;
    private RecyclerView mRecyclerView;
    private ValueService valueService;

    @Override
    protected void onCreate (Bundle savedInstanceState)  {
        super.onCreate( savedInstanceState );
        setContentView(R.layout.activity_main );
        ButterKnife.bind(this);

        valueService = ApiUtils.getValueService();
        mRecyclerView = (RecyclerView) findViewById(R.id.rv_answers);
        valueAdapter = new ValueAdapter(this, new ArrayList<Value>(0), new ValueAdapter.PostItemListener() {

            @Override
            public void onPostClick(long id) {
                Toast.makeText(MainActivity.this, "Post id is" + id, Toast.LENGTH_SHORT).show();
            }
        });

        RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(this);
        mRecyclerView.setLayoutManager(layoutManager);
        mRecyclerView.setAdapter(valueAdapter);
        mRecyclerView.setHasFixedSize(true);
        RecyclerView.ItemDecoration itemDecoration = new DividerItemDecoration(this, DividerItemDecoration.VERTICAL);
        mRecyclerView.addItemDecoration(itemDecoration);

        loadAnswers();
    }

    public void loadAnswers() {
        valueService.get().enqueue(new Callback<List<Value>>() {
            @Override
            public void onResponse(Call<List<Value>> call, Response<List<Value>> response) {

                if(response.isSuccessful()) {
                    valueAdapter.updateAnswers(response.body());
                    Log.d("MainActivity", "posts loaded from API");
                }else {
                    int statusCode  = response.code();
                    // handle request errors depending on status code
                    Log.d("MainActivity", "posts loaded from API statusCode" + statusCode);
                }
            }

            @Override
            public void onFailure(Call<List<Value>> call, Throwable t) {
                Log.d("MainActivity", "error loading from API");

            }
        });
    }


        /** Called when the user taps the Send button */
    public void sendMessage(View view) {
        // Do something in response to button
        //Log.d("myTag", buttonSend.getText().toString());
        //EditText editText = (EditText) findViewById(R.id.editText);
        //String message = editText.getText().toString();
    }


}
