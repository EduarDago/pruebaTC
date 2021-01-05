�
aG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Dominio\Entidades\CampoOrigen.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Dominio$ +
.+ ,
	Entidades, 5
{ 
public 

class 
CampoOrigen 
{		 
[

 	
JsonProperty

	 
(

 
PropertyName

 "
=

# $
$str

% )
)

) *
]

* +
public 
int 
Id 
{ 
get 
; 
set  
;  !
}" #
[
JsonProperty
(
PropertyName
=
$str
)
]
public 
int 
IdConfiguracion "
{# $
get% (
;( )
set* -
;- .
}/ 0
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public 
string 
Nombre 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% /
)/ 0
]0 1
public 
string 
TipoDato 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} �
iG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Dominio\Entidades\ConfiguracionOrigen.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Dominio$ +
.+ ,
	Entidades, 5
{ 
public 

class 
ConfiguracionOrigen $
{		 
[

 	
JsonProperty

	 
(

 
PropertyName

 "
=

# $
$str

% 6
)

6 7
]

7 8
public 
int 
IdConfiguracion "
{# $
get% (
;( )
set* -
;- .
}/ 0
[
JsonProperty
(
PropertyName
=
$str
)
]
public 
string 
Nombre 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
string 
Descripcion !
{" #
get$ '
;' (
set) ,
;, -
}. /
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 4
)4 5
]5 6
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
string 
NombreTabla !
{" #
get$ '
;' (
set) ,
;, -
}. /
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public 
IEnumerable 
< 
CampoOrigen &
>& '
Campos( .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 1
)1 2
]2 3
public 
IEnumerable 
< 
ParametroOrigen *
>* +

Parametros, 6
{7 8
get9 <
;< =
set> A
;A B
}C D
} 
} �	
gG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Dominio\Entidades\DetalleValidacion.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Dominio$ +
.+ ,
	Entidades, 5
{ 
public 

class 
DetalleValidacion "
{ 
public		 
int		 
	IdDetalle		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
public 
int 
IdValidacion 
{  !
get" %
;% &
set' *
;* +
}, -
public
string
Campo
{
get
;
set
;
}
public 
string 
Operador 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
Valor 
{ 
get !
;! "
set# &
;& '
}( )
} 
} �
_G:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Dominio\Entidades\Ejecucion.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Dominio$ +
.+ ,
	Entidades, 5
{ 
public 

class 
	Ejecucion 
{ 
public		 
int		 
IdValidacion		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		, -
public 
string 

Validacion  
{! "
get# &
;& '
set( +
;+ ,
}- .
public
string
Estado
{
get
;
set
;
}
} 
} �
jG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Dominio\Entidades\EjecucionImportacion.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Dominio$ +
.+ ,
	Entidades, 5
{ 
public 

class  
EjecucionImportacion %
{ 
public		 
int		 
IdEjecucion		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
public 
int !
IdConfiguracionOrigen (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public
string
NombreOrigen
{
get
;
set
;
}
public 
DateTime 
FechaEjecucion &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
string 
Estado 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
CantidadRegistros $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
bool 
UltimaEjecucion #
{$ %
get& )
;) *
set+ .
;. /
}0 1
} 
} �
eG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Dominio\Entidades\ParametroOrigen.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Dominio$ +
.+ ,
	Entidades, 5
{ 
public 

class 
ParametroOrigen  
{		 
[

 	
JsonProperty

	 
(

 
PropertyName

 "
=

# $
$str

% )
)

) *
]

* +
public 
int 
Id 
{ 
get 
; 
set  
;  !
}" #
[
JsonProperty
(
PropertyName
=
$str
)
]
public 
int 
IdConfiguracion "
{# $
get% (
;( )
set* -
;- .
}/ 0
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public 
string 
Nombre 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% /
)/ 0
]0 1
public 
string 
TipoDato 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} �
\G:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Dominio\Entidades\Topico.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Dominio$ +
.+ ,
	Entidades, 5
{ 
public 

class 
Topico 
{ 
public		 
int		 
IdTopico		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}0 1
public
string
Valor
{
get
;
set
;
}
public 
string 
TextoMostrar "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
int 
Orden 
{ 
get 
; 
set  #
;# $
}% &
} 
} �
`G:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Dominio\Entidades\Validacion.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Dominio$ +
.+ ,
	Entidades, 5
{ 
public 

class 

Validacion 
{ 
public		 
int		 
IdValidacion		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		, -
public 
int 
TipoValidacion !
{" #
get$ '
;' (
set) ,
;, -
}. /
public
string
Descripcion
{
get
;
set
;
}
public 
string 
Objetivo 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
BaseDatosOrigen %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
TablaOrigen !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
bool 
Activo 
{ 
get  
;  !
set" %
;% &
}' (
public 
List 
< 
DetalleValidacion %
>% &
DetalleValidacion' 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
} 
} 