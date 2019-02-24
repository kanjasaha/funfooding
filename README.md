# funfooding
 Development environment setup

SQL:

1a. Create a database named ThreeSixtyTwo. 
1b. Run the SQL scripts in sqlscripts folder(Step1 & Step2).

2a. Move the Solr folder to c:\
2b. run the run.bat file in solr/example folder. 
2c. Modify database name/username/password in C:\solr\example\solr\collection1\conf\data-config.xml
2d. Check dataimport functionality in http://localhost:8983/solr/ 

3a. Make Sure MealPhotos and ProfilePhotos have write permission.
http://www.iis.net/learn/manage/configuring-security/application-pool-identities
3b. Modify database name/username/password in \..\funfooding\web.config
3b. Build the solution and run

#github checkin

cd C:/Users/../../../funfooding

C:/Users/Desktop/desktopfiles/1FunfoodingCode/funfooding

git init
git add .
git commit -m "message on files to be pushed"
git push -u origin master

git reset --hard to clear local and pull fresh copy from server

git pull origin master. merge server and local in local before commit.
http://guides.railsgirls.com/github/

git rm -r one-of-the-directories
git commit -m "Remove duplicated directory"
git push origin master
