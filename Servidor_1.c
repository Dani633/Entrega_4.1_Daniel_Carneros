#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <fcntl.h>
#include <netinet/in.h>
#include <stdio.h>
int main(int argc, char *argv[])
{
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	char respuesta[512];
	// INICIALITZACIONS
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		error("Error al crear el socket");
	
	// Fem el bind al port
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	//asocia el socket a cualquiera de las IP de la maquina
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY); /* El fica IP local */
	//escuchamos el puerto correspondiente:
	serv_adr.sin_port = htons(9040); //AQUI SE PONE EL PUERTO
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		error("Error al bind");
	
	// Limitem el nombre de connexions pendents
	if (listen(sock_listen, 3) < 0)
		printf("Error de Listen");	
	
	//Atendemos solo 5 peticiones
	int i;
	for(i=0;i<5;i++){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf("Se ha recibido la conexión\n");
		//sock_conn es el socket que usaremos para este cliente
		
		//Bucle de atencion al cliente
		int terminar = 0;
		while (terminar==0)
		{
			//Dejamos la consulta en la variable peticion
			ret=read(sock_conn,peticion, sizeof(peticion));
			printf("Recibido\n");
			
			//Ponemos marca de fin de string para que no escriba lo que hay despues del buffer
			peticion[ret]='\0';
			
			//Escribimos la peticion en consola
			printf("La peticion es: %s\n", peticion);
			
			//que quieren
			char *p = strtok(peticion,"/");
			int codigo = atoi(p);
			char nombre[20];
			
			if (codigo !=0)
			{
				p = strtok(NULL,"/");
				strcpy(nombre, p);
				printf("Codigo: %d, Nombre: %s\n", codigo, nombre);
			}
			
			if (codigo==0)
			{
				terminar = 1;
			}

			
			if (codigo==1) //quieren la longitud del nombre
			{
				sprintf(respuesta,"%d",strlen(nombre));
			}
			else if (codigo==2) //quieren saber si el nombre es bonito
			{
				if((nombre[0]=='D') || (nombre[0]=='M'))
				{
					strcpy(respuesta,"SI");
				}
				else
				{
					strcpy(respuesta,"NO");
				}
			}
			else if (codigo==3) //quieren saber si la persona es alta
			{
				p = strtok(NULL,"/");
				float altura = atof(p);
				if (altura > 1.70)
				{
					sprintf(respuesta, "%s: eres alto/a",nombre);
				}
				else
				{
					sprintf(respuesta, "%s: eres bajo/a",nombre);
				}
			}
			else if (codigo==4) //quieren saber si el nombre es un palindromo
			{
				int longitudnombre = strlen(nombre);
				char nombreinvertido[longitudnombre];
				char nombreminusculas[longitudnombre];
				strcpy(nombreminusculas, nombre);
				nombreminusculas[0] = tolower(nombreminusculas[0]);
				printf("El nombre en minusculas es: %s\n",nombreminusculas);
				int i = longitudnombre-1;
				int j = 0;
				while (i >= 0)
				{
					nombreinvertido[j] = nombre[i];
					j++;
					i--;
				}
				nombreinvertido[strlen(nombreinvertido)-1] = tolower(nombreinvertido[strlen(nombreinvertido)-1]);
				printf("El nombre invertido en minusculas es: %s\n",nombreinvertido);
				
				if (0 == strcmp(nombreminusculas,nombreinvertido))
				{
					sprintf(respuesta, "%s: Tu nombre es un palindromo",nombre);
				}
				else
				{
					sprintf(respuesta, "%s: Tu nombre NO es un palindromo",nombre);
				}
			}
			else if (codigo==5) //quieren saber si el nombre es un palindromo
			{
				for (int index = 0; nombre[index] != '\0'; ++index)
				{
					nombre[index] = toupper(nombre[index]);
				}
				sprintf(respuesta, "Tu nombre en mayusculas es: %s\n",nombre);
			}
			
			if (codigo !=0)
			{
				printf("Respuesta: %s\n", respuesta);
				write(sock_conn,respuesta,strlen(respuesta));
			}
		}
		
		close(sock_conn); /* Necessari per a que el client detecti EOF */
	}
}
		
		
		
		
		
		
		
		
		
