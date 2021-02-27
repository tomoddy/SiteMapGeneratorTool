#!/bin/sh

# Decrypt the file
mkdir $HOME/secrets
# --batch to prevent interactive command
# --yes to assume "yes" for questions
gpg --quiet --batch --yes --decrypt --passphrase="$APP_SETTINGS_PASSWORD" \
--output $HOME/secrets/my_secret.json my_secret.json.gpg