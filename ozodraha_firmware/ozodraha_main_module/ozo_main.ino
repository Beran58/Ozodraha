#include <Timer.h>
Timer timer;
int ledPin1 = 5;
int ledPin2 = 6;
int ledPin3 = 7;
int interval = 5;
int elapsed= 0;
bool o = true;
void setup()
{
pinMode(ledPin1,OUTPUT);
pinMode(ledPin2,OUTPUT);
pinMode(ledPin3,OUTPUT);
timer.start();
Serial.begin(9600);
}

void loop()
{
if(timer.read() >= elapsed+interval)
{
o = flick(ledPin1,ledPin2,ledPin3,o);
elapsed = timer.read();
}
}

bool flick(int led1,int led2,int led3, bool u)
{
  if(u == true)
  {
    digitalWrite(led1,HIGH);
    digitalWrite(led2,HIGH);
    digitalWrite(led3,HIGH);
    return false;
  }
  else
  digitalWrite(led1,LOW);
  digitalWrite(led2,LOW);
  digitalWrite(led3,LOW);
  return true; 
}