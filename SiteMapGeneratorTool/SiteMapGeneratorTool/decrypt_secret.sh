gpg --quiet --batch --yes --decrypt --passphrase="$APP_SETTINGS_PASSWORD" --output SiteMapGeneratorTool/SiteMapGeneratorTool/appsettings.json SiteMapGeneratorTool/SiteMapGeneratorTool/appsettings.json.gpg
gpg --quiet --batch --yes --decrypt --passphrase="$APP_SETTINGS_PASSWORD" --output SiteMapGeneratorTool/SiteMapGeneratorTool/Keys/firebase.json SiteMapGeneratorTool/SiteMapGeneratorTool/Keys/firebase.json.gpg