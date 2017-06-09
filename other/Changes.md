# Interaktivni projekt s pomočjo uporabe konvolucijskih nevronskih mrež (CNN) #

### Changes ###
#### neural v0.4 Changes ####
- dodaj izbiro detekcije
- dodaj dodaj izbiro slogu
- seminar:
	- predstavi user case
	- nadaljne delo

#### neural v0.3 Changes ####
- lastno compilanje knjižnic za uporabo nevronskih mrež
- dokončanje backaenda:
	- dodajanje servicev za REST API

#### neural v0.2 Changes ####
- hitrejša detekcija z preddefiniranim stilom:
	- https://github.com/lengstrom/fast-style-transfer
	- https://github.com/jcjohnson/fast-neural-style
	- https://github.com/yusuketomoto/chainer-fast-neuralstyle

#### neural v0.1 Changes ####
- BUTLER: 
	- asp.net core strežnik
	- konfiguracija ASP.NET CORE strežnika
	- ImageController - implementacija POST poizvedbe z sliko na strežnik
	- priprava NGINX daemona za publish
	- BASH: oddajanje skripte za avtomatski publish

- DETECTOR:
	- implementacija komunikacije z BUTLERjem preko STDIN/STDOUT
	- prilagoditev C kode
	
- BUTLER/DETECTOR:
	- komunikacija med procesoma
	- pošiljanje slike detectorju
	- implementacija vrste (queue)
	- čakanje na zaključek obdelave v detectorju
	- vračanje detektirane slike uporabniku, kot odgovor na POST poizvedbo
	- implementacija GetById
 
- ARTIST:
    - priprava service-a na BUTLER strežniku
	- prilagoditev python kode
	
- BUTLER/DETECTOR:
	- komunikacija med procesoma
	
PROBLEMI:
	- počasnost CNN

