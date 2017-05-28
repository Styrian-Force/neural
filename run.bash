#!/usr/bin/env bash

case $1 in
    "p"|"publish")
    	sudo systemctl restart nginx
        cd butler
	dotnet clean
	dotnet publish
	sudo cp -r /home/administrator/dev/git/neural/butler/butler/bin/Debug/netcoreapp1.1/publish/* /var/aspnetcore/butler/
	sudo systemctl restart butler.service
        sudo journalctl -fu butler.service
        ;;    
    *)
        cat << EOF
p|publish
EOF
        ;;
esac
