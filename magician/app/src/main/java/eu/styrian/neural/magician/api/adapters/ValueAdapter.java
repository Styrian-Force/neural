package eu.styrian.neural.magician.api.adapters;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import java.util.List;

import eu.styrian.neural.magician.api.models.Value;

public class ValueAdapter extends RecyclerView.Adapter<ValueAdapter.ViewHolder> {

    private List<Value> mValues;
    private Context mContext;
    private PostItemListener mItemListener;

    public class ViewHolder extends RecyclerView.ViewHolder implements View.OnClickListener{

        public TextView titleTv;
        PostItemListener mItemListener;

        public ViewHolder(View itemView, PostItemListener postItemListener) {
            super(itemView);
            titleTv = (TextView) itemView.findViewById(android.R.id.text1);

            this.mItemListener = postItemListener;
            itemView.setOnClickListener(this);
        }

        @Override
        public void onClick(View view) {
            Value item = getItem(getAdapterPosition());
            this.mItemListener.onPostClick(0);

            notifyDataSetChanged();
        }
    }

    public ValueAdapter(Context context, List<Value> posts, PostItemListener itemListener) {
        mValues = posts;
        mContext = context;
        mItemListener = itemListener;
    }

    @Override
    public ValueAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

        Context context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(context);

        View postView = inflater.inflate(android.R.layout.simple_list_item_1, parent, false);

        ViewHolder viewHolder = new ViewHolder(postView, this.mItemListener);
        return viewHolder;
    }

    @Override
    public void onBindViewHolder(ValueAdapter.ViewHolder holder, int position) {

        Value value = mValues.get(position);
        TextView textView = holder.titleTv;
        textView.setText(value.getCode());
    }

    @Override
    public int getItemCount() {
        return mValues.size();
    }

    public void updateAnswers(List<Value> items) {
        mValues = items;
        notifyDataSetChanged();
    }

    private Value getItem(int adapterPosition) {
        return mValues.get(adapterPosition);
    }

    public interface PostItemListener {
        void onPostClick(long id);
    }
}