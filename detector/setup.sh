weightsDir="weights/"
if [ -d "$weightsDir" ]; then
  	rm -rf $weightsDir 
fi

mkdir $weightsDir
wget https://pjreddie.com/media/files/yolo.weights -P $weightsDir
#wget https://pjreddie.com/media/files/tiny-yolo.weights -P $weightsDir

cd data/labels
find . ! -name 'make_labels.py' -type f -exec rm -f {} +
python3 make_labels.py
cd ../..
