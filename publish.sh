set -e

# Configuration...
packageDir="./MicroDI/Assets/MicroDI"
feed="feed.redgames.studio"

echo -e "\e[35mFetching NPM Auth Token...\e[0m" 
json="{\"name\": \""$NPM_USERNAME"\", \"password\": \""$NPM_PASSWORD"\"}"
TOKEN=$(curl -s -H "Accept: application/json" -H "Content-Type:application/json" -X PUT --data "$json" http://$feed/-/user/org.couchdb.user:$NPM_USERNAME 2>&1 | grep -Po '(?<="token": ")[^"]*')

# Setup npm auth...
npm set registry "http://$feed/"
npm set //$feed/:_authToken $TOKEN

# Increment versioning...
echo -e "\e[35mIncrementing package version (standard-version)...\e[0m"
git config --local user.email "action@github.com"
git config --local user.name "GitHub Action"
npm i -g standard-version
cd $packageDir && standard-version

# Publish...
echo -e "\e[35mPublishing...\e[0m"
npm publish

# Push version tag...
echo -e "\e[35mPushing version tags...\e[0m"
git push --follow-tags origin