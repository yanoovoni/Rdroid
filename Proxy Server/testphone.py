from socket import *
from Printer import *
from Settings import *
from Encryptor import *
from Crypto.Cipher import AES
import base64

settings = Settings()
encryptor = Encryptor(AES.new(b'01234567890123456789012345678901'))
sock = socket()
sock.connect(('127.0.0.1', int(settings.getSetting('my_port'))))
print 'connected'
key = sock.recv(1028)

sock.send(base64.b64encode(encryptor.encrypt('Rdroid CLIENT\nyou be faggot now')))
print 'sent'
print encryptor.decrypt(sock.recv(1028))