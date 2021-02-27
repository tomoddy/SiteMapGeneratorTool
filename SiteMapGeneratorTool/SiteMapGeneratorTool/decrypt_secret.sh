gpg --quiet --batch --yes --decrypt --passphrase="$APP_SETTINGS_PASSWORD" \
--output appsettings.json appsettings.json.gpg