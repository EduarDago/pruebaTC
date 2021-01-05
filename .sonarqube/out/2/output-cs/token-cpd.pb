˜
eG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Proxy\BL\ConfiguracionOrigenProxy.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Proxy$ )
.) *
BL* ,
{		 
public

 

class

 $
ConfiguracionOrigenProxy

 )
{ 
private 
readonly $
ConfiguracionOrigenDatos 1
datos2 7
;7 8
public $
ConfiguracionOrigenProxy '
(' (
IConfiguration( 6
config7 =
)= >
{ 	
datos 
= 
new $
ConfiguracionOrigenDatos 0
(0 1
config1 7
)7 8
;8 9
} 	
public 
List 
< 
ConfiguracionOrigen '
>' ((
ObtenerConfiguracionesOrigen) E
(E F
)F G
{ 	
return 
datos 
. (
ObtenerConfiguracionesOrigen 5
(5 6
)6 7
;7 8
} 	
public!! 
ConfiguracionOrigen!! "&
ObtenerConfiguracionOrigen!!# =
(!!= >
int!!> A
idConfiguracion!!B Q
)!!Q R
{"" 	
return## 
datos## 
.## &
ObtenerConfiguracionOrigen## 3
(##3 4
idConfiguracion##4 C
)##C D
;##D E
}$$ 	
public++ 
List++ 
<++  
EjecucionImportacion++ (
>++( ) 
ConsultarEjecuciones++* >
(++> ?
)++? @
{,, 	
return-- 
datos-- 
.--  
ConsultarEjecuciones-- -
(--- .
)--. /
;--/ 0
}.. 	
public55  
EjecucionImportacion55 #
EjecutarImportacion55$ 7
(557 8
int558 ;
idConfiguracion55< K
)55K L
{66 	
return77 
datos77 
.77 
EjecutarImportacion77 ,
(77, -
idConfiguracion77- <
)77< =
;77= >
}88 	
public?? 
bool?? '
InsertarConfiguracionOrigen?? /
(??/ 0
ConfiguracionOrigen??0 C
configuracion??D Q
)??Q R
{@@ 	
returnAA 
datosAA 
.AA '
InsertarConfiguracionOrigenAA 4
(AA4 5
configuracionAA5 B
)AAB C
;AAC D
}BB 	
}CC 
}DD ð
XG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Proxy\BL\TopicoProxy.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Proxy$ )
.) *
BL* ,
{		 
public

 

class

 
TopicoProxy

 
{ 
private 
readonly 
TopicoDatos $
datos% *
;* +
public 
TopicoProxy 
( 
IConfiguration )
config* 0
)0 1
{ 	
datos 
= 
new 
TopicoDatos #
(# $
config$ *
)* +
;+ ,
} 	
public 
List 
< 
Topico 
> 
ObtenerTopicos *
(* +
string+ 1
identificador2 ?
)? @
{ 	
return 
datos 
. 
ObtenerTopicos '
(' (
identificador( 5
)5 6
;6 7
} 	
} 
} È
\G:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Proxy\BL\ValidacionProxy.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Proxy$ )
.) *
BL* ,
{		 
public

 

class

 
ValidacionProxy

  
{ 
private 
readonly 
ValidacionDatos (
datos) .
;. /
public 
ValidacionProxy 
( 
IConfiguration -
config. 4
)4 5
{ 	
datos 
= 
new 
ValidacionDatos '
(' (
config( .
). /
;/ 0
} 	
public 
List 
< 

Validacion 
> 
ObtenerValidaciones  3
(3 4
)4 5
{ 	
return 
datos 
. 
ObtenerValidaciones ,
(, -
)- .
;. /
} 	
public"" 

Validacion"" 
ObtenerValidacion"" +
(""+ ,
int"", /
idValidacion""0 <
,""< =
bool""> B
incluirDetalle""C Q
)""Q R
{## 	
return$$ 
datos$$ 
.$$ 
ObtenerValidacion$$ *
($$* +
idValidacion$$+ 7
,$$7 8
incluirDetalle$$9 G
)$$G H
;$$H I
}%% 	
public(( 
bool(( 
EjecutarValidacion(( &
(((& '
int((' *
idValidacion((+ 7
)((7 8
{)) 	
return** 
datos** 
.** 
EjecutarValidacion** +
(**+ ,
idValidacion**, 8
)**8 9
;**9 :
}++ 	
},, 
}-- 