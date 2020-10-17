# PTS
 
Dans le cadre de notre PTS (Projet Technique et Scientifique) de 4ème année, différents sujets nous ont été proposés. Nous avons choisi de nous tourner vers un problème d’optimisation mathématique, l’idée de travailler sur un sujet avec de nombreuses solutions existante, et être dans une démarche de recherche pour essayer de trouver autre chose, de peut-être plus performant nous plaisait beaucoup. 
Nous avons donc choisi le "Traitement du problème du voyageur de commerce par les algorithmes génétiques".


## Presentation du problème

Le PVC (Problème du Voyageur de Commerce) est un problème mathématique bien connu. Le principe est le suivant :

Un voyageur de commerce doit visiter une liste de villes pour revenir à son points de départ en passant une et une seule fois par chaque ville. Il faut par consequant trouver le trajet permettant de minimiser la distance totale parcourue par le voyageur.

Si ça a l’air plutôt simple au premier abord, le nombre de trajets possibles pour n villes est n! (factorielle n). Trouver la meilleure solution s’avère donc bien ardu au fur et à mesure que le nombre de villes à parcourir augmente. Si bien qu’on est obligé de passer par des algorithmes pour le faire.

Du fait de sa complexité, le PVC appartient à la famille des problèmes dits NP-complets (un problème complet pour la classe NP - Non deterministic Polynomial). En théorie de la complexité, un problème NP-complet est un problème de décision vérifiant les propriétés suivantes :
 
- il est possible de vérifier une solution efficacement (en temps polynomial) ; la classe des problèmes vérifiant cette propriété est notée NP ;
- tous les problèmes de la classe NP se ramènent à celui-ci via une réduction polynomiale ; cela signifie que le problème est au moins aussi difficile que tous les autres problèmes de la classe NP.

Par conséquent, il nous est donc impossible de trouver la solution exacte en temps polynomial. Pour résoudre ce genre de problèmes, nous devons leurs appliquer des algorithmes nous permettant de trouver une solution optimale en un temps donné. Ce type d'algorithmes n'est pas déterministe, donc nous ne trouverons pas la même solution à tout les coups.


## Solution

Le sujet choisi pour notre PTS, nous impose de résoudre ce problème en utilisant un [algorithme génétique](https://fr.wikipedia.org/wiki/Algorithme_génétique). Pour se faire, nous avons décidé dans un premier temps, de comparer les algorithmes génétiques les plus performants et par la suite mettre en place notre propre algorithme grâce à ce que l'on aura appris.


### Les Algorithmes

Après nos recherches nous avons choisi d'implémenter 2 algorythmes afin de les comparer : 

- Sélection par tournoi :
C’est la méthode donnant en général les meilleurs résultats. Elle possède un paramètre T, la taille du tournoi. Le principe est le suivant : on effectue un tirage de T individus dans la population et chaque tirage donne lieu à un combat. L’individu dont la fonction fitness est la plus élevée, donc celui qui a remporté «le combat» est désigné comme parent. On reproduit ce processus autant de fois que nécessite l’obtention des n nouveaux individus de la 2e génération.

- Sélection élitiste :
C’est la seule méthode de sélection qui soit déterministe. Son principe est de classer par ordre croissant les individus en fonction de leur fitness. Ensuite, on sélectionne un ou plusieurs individus parmi les meilleurs de ce classement qui constitueront la population de parents. On génère ensuite par croisement les individus enfants nécessaires à la constitution de la génération suivante. Les individus les moins performants sont totalement éliminés de la population, et le meilleur individu est toujours sélectionné – on dit que cette sélection est élitiste.
Cette méthode présente une convergence fortement prématurée car on a une variance et une diversité presque nulles : les individus les moins bons n’ont aucune chance de survivre. Elle permet cependant de ne pas perdre des individus avec une fonction fitness élevée.

Par la suite ces deux algorithmes seront comparés à notre propre algorithme, appelé Sélection before. Les sélections tournoi et élitiste nous servirons de méthode témoin, afin de pouvoir évaluer la performance de notre algorithme.


### Sélection Before

#### Initialisation

Tout d’abord, en guise de préliminaires, nous avons décidé que plutôt que de choisir aléatoirement la population initiale, nous choisirions 2 trajets optimisés(trajets avec des distances totales pas trop mauvaises). L’idée est de générer grâce à ces deux trajets, une première population par un algorithme que l’on a baptisé [Adam et Eve](https://github.com/WellsL/PTS/blob/master/Rapport-PTS-Groupe10.pdf).

#### Sélection

Une fois la population initiale obtenue, nous allons procéder à la prochaine étape : Le Croisement.

1) On commence par trier les trajets par fitness. (Rappel : la fitness est la distance en kilomètres, donc on cherche la meilleure fitness qui sera la plus petite).

2) On divise ensuite la population en deux. Les 15% meilleurs constitueront notre première sous-population (notons PopA) et les 85% restants la population la sous- population B (notons PopB).

####  Croisement

L’idée est de faire se reproduire les bons individus avec les moins bons. Contrairement à la méthode élitiste où l’on ne garde que les meilleurs, nous avons décidé de procéder ainsi car il se peut qu’un parent a priori mauvais possède tout de même un gène (une partie de trajet)
puissant. Ainsi, on garde une chance de récupérer de bons éléments.

1) Chaque individu de PopA se reproduit avec un individu de PopB. L’heureux élu est déterminé par une sélection stochastique sur PopB pour accroître la part de hasard. Le croisement se fait avec un taux de mutation élevé pour éviter de stagner.

2) On répète l’opération autant que nécessaire pour obtenir le nombre n d’individus désiré.

Ainsi, on a obtenu la population de n individus de la 1ère génération !

Pour créer les générations suivantes, on reprend à partir de la sélection (l’initialisation ne se fait qu’une seule fois au tout début).

### API / Interface web

Afin de comparer les différentes sélections, nous avons décidé de mettre en place une interface web en ReactJS, et une API en C#.
Cette interface nous permettra de visualiser facilement les résultats via des graphiques, elle facilitera aussi le choix des algorithmes à comparer, ainsi que la liste des villes à visiter. 

Pour être sûr de terminer en temps et en heure, et d'avoir un produit à présenter, nous avons utiliser le concepte [MVP (Minimum Viable Product)](https://fr.wikipedia.org/wiki/Produit_minimum_viable).

Nous avons suivit le workflow suivant :

1) Recherche d'une base de données

2) Création d'une base de données MySQL + Importation des données

3) Développement de l'application console (App) : permettant de comparer les différentes méthodes de selections

4) Développement de l'API (App) : permettant d'exposer l'application (App) grâce à des EndPoints

5) Développement de l'interface Web (ClientApp)
