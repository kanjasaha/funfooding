# funfooding
 Development environment setup

SQL:
1a. Run the scripts in sqlscripts folder.
1b. Ensure SQL Login is enabled and has access to the database.

2a. Move the Solr folder to c:\
2b. run the run.bat file in solr/example folder. 
2c. Modify database name/username/password in C:\solr\example\solr\collection1\conf\data-config.xml
2d. Check dataimport functionality in http://localhost:8983/solr/ 

3a. Make Sure MealPhotos and ProfilePhotos have write permission.
http://www.iis.net/learn/manage/configuring-security/application-pool-identities
3b. Build the solution and run