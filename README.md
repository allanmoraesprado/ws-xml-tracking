This is a windows service application which takes a specified client data and check every 10 minutes if the e-mail from that client received a new xml.
The xml will be saved on a directory, also it'll create a backup on another folder.
Once the application got the xml, the xml will be deserialized and a new product will be built with the xml data and be filled with the client data, then the product will be serilized as a json.
Once the service got the json, it'll be sent to an api. The api will return a response, if the response is true, the xml file from the directory will be moved to another folder which contains all processed xmls, then the old xml file will be deleted.
If the response is false, the xml will be sent to another folder which contains all not processed xmls, and deletes the old xml file.
