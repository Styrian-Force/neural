#!/usr/bin/env bash

function publish {
	sudo systemctl restart nginx
        cd butler
	dotnet clean
	dotnet publish
	butlerServiceDir="/var/aspnetcore/butler/"
	if [ ! -d "$butlerServiceDir" ]; then
	  	sudo mkdir $butlerServiceDir -p
	fi
	sudo cp -r /home/administrator/dev/git/neural/butler/butler/bin/Debug/netcoreapp1.1/publish/* $butlerServiceDir
	sudo systemctl restart butler.service
}

case $1 in
	"p"|"publish")
    	publish
        ;;
	"l"|"logs")
        sudo journalctl -fu butler.service
        ;; 
	"pl"|"publish-logs")
    	publish
        sudo journalctl -fu butler.service
        ;; 
    *)
        cat << EOF
p|publish
l|logs
pl|publish-logs
EOF
        ;;
esac
