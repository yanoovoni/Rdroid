from socket import *
from Printer import *
from Settings import *
from Encryptor import *
from Crypto.Cipher import AES
import base64
from Crypto.PublicKey import RSA
from Crypto.Cipher import PKCS1_v1_5

settings = Settings()
encryptor = Encryptor(AES.new(b'01234567890123456789012345678901'))
sock = socket()
sock.connect(('127.0.0.1', int(settings.getSetting('my_port'))))
print 'connected'
key = base64.b64decode(sock.recv(1028))
rsa = PKCS1_v1_5.new(RSA.importKey(key))
sock.send(base64.b64encode(rsa.encrypt(b'01234567890123456789012345678901')))
print 'sent'
print encryptor.decrypt(sock.recv(1028))