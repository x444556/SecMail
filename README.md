# SecMail

***NOTE:***
  - **This project is still at a very early stage and the server may not always be reachable. Due to bug fixes or changes to the protocols, messages or other user data may be lost.**
  - **Your antivirus program may incorrectly mark the client as a virus. You can ignore this (depending on which antivirus you are using, you may have to manually allow the client before using it!).**
  - **The source code may not be complete! Parts may be shortened, outdated or will only be published in the future.**

The size of attachments is limited to 32 mib.

If you have any questions or problems, open an issue on Github or write a message to ":Support".

To save your contacts, you have to create a text file with the name "contacts.txt" (or only "contacts" if the file extensions are hidden) and enter one contact per line.

**Encryption**

To ensure the security of your messages, a public and a private 2048 bit long key for the RSA algorithm is generated for each address you create. Your public key is transmitted to the server along with your address and can be queried by clients to encrypt messages to you. Your private key is stored locally on your device and is used to decrypt the messages. To ensure that the sent message also comes from the specified sender, the message is digitally signed. The private key is used for this. Once the server receives the message, it verifies the signature using the public key and discards the message if the signature is incorrect. The content of the message is encrypted with AES 256 bit and the key for this is then RSA-encrypted and sent along with the encrypted data.
