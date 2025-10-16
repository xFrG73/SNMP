# SNMP Tool - Guide d'utilisation

Outil Windows pour requêtes SNMP avec scanner réseau intégré.

## Utilisation rapide

1. **Télécharger** et lancer SNMPTool.exe
2. **Scanner réseau**: Cliquer sur la loupe pour trouver des périphériques
3. **Ou saisir une IP** manuellement
4. **Choisir un OID** dans la liste ou saisir manuellement
5. **Cliquer "Démarrer"** pour la requête

## Scanner réseau

- Détection automatique du réseau local
- Scan SNMP sur la plage d'adresses
- Sélection d'un périphérique trouvé

## OIDs prédéfinis

- **Système**: Description, nom, uptime, contact
- **Mémoire**: Mémoire physique totale
- **Disque**: Taille et utilisation du disque C:
- **Réseau**: Interfaces, vitesse, trafic

## Configuration SNMP Windows

Pour tester localement:

1. Panneau de configuration > Fonctionnalités Windows > Cocher "Protocole SNMP"
2. services.msc > Service SNMP > Propriétés > Sécurité
3. Ajouter "public" dans les communautés
4. Démarrer le service

## Dépannage

- **Aucun périphérique trouvé**: Vérifier que SNMP est activé
- **OID non supporté**: L'OID n'existe pas sur ce périphérique
- **Aucune réponse**: Vérifier l'IP et la connectivité réseau

## Interface

<img width="436" height="444" alt="{FC578A0E-72A7-4BB8-A800-1EACA1207C68}" src="https://github.com/user-attachments/assets/93ca6ddf-205f-4e3b-bbc2-0e67e682dfc4" />

## Exemples

**Test local**: IP=127.0.0.1, OID="Description du système"
**Scan réseau**: Cliquer loupe, scanner, sélectionner périphérique
**Monitoring**: Choisir "Interface 1 - Octets reçus" pour voir le trafic