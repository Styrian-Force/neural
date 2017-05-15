# Interaktivni projekt s pomočjo uporabe konvolucijskih nevronskih mrež (CNN) #

This README would normally document whatever steps are necessary to get your application up and running.

### Teoretično ozadje ###

#### Članek na Overleaf ####
* [Povezava na urejanje članka - Seminar 1](https://www.overleaf.com/8783020xmvxrqdxrjjx)
* [Povezava na urejanje članka - Seminar 2](https://www.overleaf.com/9568204fvzkzkfykwbz)

### Changes ###

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

### What is this repository for? ###

* Quick summary
* Version
* [Learn Markdown](https://bitbucket.org/tutorials/markdowndemo)

### How do I get set up? ###

* Summary of set up
* Configuration
* Dependencies
* Database configuration
* How to run tests
* Deployment instructions

### Contribution guidelines ###

* Writing tests
* Code review
* Other guidelines

### Who do I talk to? ###

* Repo owner or admin
* Other community or team contact