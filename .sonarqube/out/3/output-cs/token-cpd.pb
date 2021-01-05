¶
kG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Presentacion\Controllers\HomeController.cs
	namespace		 	

Proteccion		
 
.		 
TableroControl		 #
.		# $
Presentacion		$ 0
.		0 1
Controllers		1 <
{

 
public 

class 
HomeController 
:  !

Controller" ,
{ 
public 
IActionResult 
Index "
(" #
)# $
{ 	
return 
View 
( 
) 
; 
} 	
public 
IActionResult 
About "
(" #
)# $
{ 	
ViewData 
[ 
$str 
] 
=  !
$str" F
;F G
return 
View 
( 
) 
; 
} 	
public 
IActionResult 
Contact $
($ %
)% &
{ 	
ViewData 
[ 
$str 
] 
=  !
$str" 6
;6 7
return 
View 
( 
) 
; 
} 	
public   
IActionResult   
Privacy   $
(  $ %
)  % &
{!! 	
return"" 
View"" 
("" 
)"" 
;"" 
}## 	
[%% 	
ResponseCache%%	 
(%% 
Duration%% 
=%%  !
$num%%" #
,%%# $
Location%%% -
=%%. /!
ResponseCacheLocation%%0 E
.%%E F
None%%F J
,%%J K
NoStore%%L S
=%%T U
true%%V Z
)%%Z [
]%%[ \
public&& 
IActionResult&& 
Error&& "
(&&" #
)&&# $
{'' 	
return(( 
View(( 
((( 
new(( 
ErrorViewModel(( *
{((+ ,
	RequestId((- 6
=((7 8
Activity((9 A
.((A B
Current((B I
?((I J
.((J K
Id((K M
??((N P
HttpContext((Q \
.((\ ]
TraceIdentifier((] l
}((m n
)((n o
;((o p
})) 	
}** 
}++ «!
mG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Presentacion\Controllers\OrigenController.cs
	namespace

 	

Proteccion


 
.

 
TableroControl

 #
.

# $
Presentacion

$ 0
.

0 1
Controllers

1 <
{ 
public 

class 
OrigenController !
:" #

Controller$ .
{ 
private 
readonly $
ConfiguracionOrigenProxy 1
origenProxy2 =
;= >
public 
OrigenController 
(  
IConfiguration  .
config/ 5
)5 6
{ 	
origenProxy 
= 
new $
ConfiguracionOrigenProxy 6
(6 7
config7 =
)= >
;> ?
} 	
public 
IActionResult 
Index "
(" #
)# $
{ 	
var 
origenes 
= 
origenProxy &
.& '(
ObtenerConfiguracionesOrigen' C
(C D
)D E
;E F
return 
View 
( 
origenes  
)  !
;! "
} 	
public## 
IActionResult## 
Nuevo## "
(##" #
)### $
{$$ 	
return%% 
View%% 
(%% 
new%% 
ConfiguracionOrigen%% /
(%%/ 0
)%%0 1
)%%1 2
;%%2 3
}&& 	
[-- 	
HttpPost--	 
]-- 
public.. 
IActionResult.. 
Nuevo.. "
(.." #
ConfiguracionOrigen..# 6
configuracion..7 D
)..D E
{// 	
return00 
View00 
(00 
)00 
;00 
}11 	
public88 
IActionResult88 
Detalle88 $
(88$ %
int88% (
id88) +
)88+ ,
{99 	
var:: 
origen:: 
=:: 
origenProxy:: $
.::$ %&
ObtenerConfiguracionOrigen::% ?
(::? @
id::@ B
)::B C
;::C D
return;; 
View;; 
(;; 
origen;; 
);; 
;;;  
}<< 	
publicCC 
IActionResultCC 
EditarCC #
(CC# $
intCC$ '
idCC( *
)CC* +
{DD 	
varEE 
origenEditarEE 
=EE 
origenProxyEE *
.EE* +&
ObtenerConfiguracionOrigenEE+ E
(EEE F
idEEF H
)EEH I
;EEI J
returnFF 
ViewFF 
(FF 
origenEditarFF $
)FF$ %
;FF% &
}GG 	
publicII 
IActionResultII 
EliminarII %
(II% &
intII& )
idII* ,
)II, -
{JJ 	
varKK 
origenesKK 
=KK 
origenProxyKK &
.KK& '&
ObtenerConfiguracionOrigenKK' A
(KKA B
idKKB D
)KKD E
;KKE F
returnLL 
ViewLL 
(LL 
origenesLL  
)LL  !
;LL! "
}MM 	
publicSS 
IActionResultSS 
ImportarSS %
(SS% &
)SS& '
{TT 	
varUU 
origenesUU 
=UU 
origenProxyUU &
.UU& ' 
ConsultarEjecucionesUU' ;
(UU; <
)UU< =
;UU= >
returnVV 
ViewVV 
(VV 
origenesVV  
)VV  !
;VV! "
}WW 	
[YY 	
HttpPostYY	 
]YY 
publicZZ 
IActionResultZZ !
InsertarConfiguracionZZ 2
(ZZ2 3
ConfiguracionOrigenZZ3 F
configuracionZZG T
)ZZT U
{[[ 	
var\\ 
	resultado\\ 
=\\ 
origenProxy\\ '
.\\' ('
InsertarConfiguracionOrigen\\( C
(\\C D
configuracion\\D Q
)\\Q R
;\\R S
return]] 
Json]] 
(]] 
	resultado]] !
)]]! "
;]]" #
}^^ 	
}__ 
}`` “
nG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Presentacion\Controllers\TableroController.cs
	namespace

 	

Proteccion


 
.

 
TableroControl

 #
.

# $
Presentacion

$ 0
.

0 1
Controllers

1 <
{ 
public 

class 
TableroController "
:# $

Controller% /
{ 
private 
readonly 
TopicoProxy $
topicoProxy% 0
;0 1
private 
readonly 
ValidacionProxy (
validacionProxy) 8
;8 9
public 
TableroController  
(  !
IConfiguration! /
config0 6
)6 7
{ 	
topicoProxy 
= 
new 
TopicoProxy )
() *
config* 0
)0 1
;1 2
validacionProxy 
= 
new !
ValidacionProxy" 1
(1 2
config2 8
)8 9
;9 :
} 	
[ 	
HttpGet	 
] 
public 
IActionResult 
Index "
(" #
)# $
{ 	
var 
	viewModel 
= 
new 
TableroViewModel  0
(0 1
)1 2
;2 3
	viewModel 
. 
Tipos 
= 
topicoProxy )
.) *
ObtenerTopicos* 8
(8 9
$str9 I
)I J
;J K
	viewModel 
. 
Validaciones "
=# $
validacionProxy% 4
.4 5
ObtenerValidaciones5 H
(H I
)I J
;J K
return 
View 
( 
	viewModel !
)! "
;" #
} 	
[!! 	
HttpGet!!	 
]!! 
public"" 
IActionResult"" 
DetalleValidacion"" .
("". /
int""/ 2
idValidacion""3 ?
)""? @
{## 	
var$$ 

validacion$$ 
=$$ 
validacionProxy$$ ,
.$$, -
ObtenerValidacion$$- >
($$> ?
idValidacion$$? K
,$$K L
true$$M Q
)$$Q R
;$$R S
return%% 
PartialView%% 
(%% 
$str%% 3
,%%3 4

validacion%%5 ?
)%%? @
;%%@ A
}&& 	
}'' 
}(( œ
lG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Presentacion\Helpers\HtmlHelperExtension.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Presentacion$ 0
.0 1
Helpers1 8
{ 
public 

static 
class 
HtmlHelperExtension +
{ 
public 
static 

HtmlString  
HtmlConvertToJson! 2
(2 3
this3 7
IHtmlHelper8 C

htmlHelperD N
,N O
objectP V
modelW \
)\ ]
{ 	
var 
settings 
= 
new "
JsonSerializerSettings 5
(5 6
)6 7
;7 8
settings 
. !
ReferenceLoopHandling *
=+ ,!
ReferenceLoopHandling- B
.B C
IgnoreC I
;I J
settings 
. 

Formatting 
=  !

Formatting" ,
., -
Indented- 5
;5 6
return 
new 

HtmlString !
(! "
JsonConvert" -
.- .
SerializeObject. =
(= >
model> C
,C D
settingsE M
)M N
)N O
;O P
} 	
} 
} §
eG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Presentacion\Hubs\NotificacionHub.cs
	namespace

 	

Proteccion


 
.

 
TableroControl

 #
.

# $
Presentacion

$ 0
.

0 1
Hubs

1 5
{ 
public 

class 
NotificacionHub  
:! "
Hub# &
{ 
private 
readonly 
ValidacionProxy (
validacionProxy) 8
;8 9
private 
readonly $
ConfiguracionOrigenProxy 1
origenProxy2 =
;= >
public 
NotificacionHub 
( 
IConfiguration -
config. 4
)4 5
{ 	
validacionProxy 
= 
new !
ValidacionProxy" 1
(1 2
config2 8
)8 9
;9 :
origenProxy 
= 
new $
ConfiguracionOrigenProxy 6
(6 7
config7 =
)= >
;> ?
} 	
public 
void  
EjecutarValidaciones (
(( )
)) *
{ 	
var 
validaciones 
= 
validacionProxy .
.. /
ObtenerValidaciones/ B
(B C
)C D
;D E
Parallel 
. 
ForEach 
( 
validaciones )
,) *
async+ 0
(1 2

validacion2 <
)< =
=>> @
{ 
validacionProxy 
.  
EjecutarValidacion  2
(2 3

validacion3 =
.= >
IdValidacion> J
)J K
;K L
await 
Clients 
. 
All !
.! "
	SendAsync" +
(+ ,
$str, @
,@ A
newB E
{F G
idH J
=K L

validacionM W
.W X
IdValidacionX d
,d e
	resultadof o
=p q
$str	r †
}
‡ ˆ
)
ˆ ‰
;
‰ Š
} 
) 
; 
Clients!! 
.!! 
All!! 
.!! 
	SendAsync!! !
(!!! "
$str!!" 9
)!!9 :
;!!: ;
}"" 	
public$$ 
void$$ !
EjecutarImportaciones$$ )
($$) *
)$$* +
{%% 	
var&& 
origenes&& 
=&& 
origenProxy&& &
.&&& ' 
ConsultarEjecuciones&&' ;
(&&; <
)&&< =
;&&= >
Parallel'' 
.'' 
ForEach'' 
('' 
origenes'' %
,''% &
async''' ,
(''- .
origen''. 4
)''4 5
=>''6 8
{(( 
var)) 
	resultado)) 
=)) 
origenProxy))  +
.))+ ,
EjecutarImportacion)), ?
())? @
origen))@ F
.))F G!
IdConfiguracionOrigen))G \
)))\ ]
;))] ^
await** 
Clients** 
.** 
All** !
.**! "
	SendAsync**" +
(**+ ,
$str**, B
,**B C
	resultado**D M
)**M N
;**N O
}++ 
)++ 
;++ 
Clients.. 
... 
All.. 
... 
	SendAsync.. !
(..! "
$str.." ;
)..; <
;..< =
}// 	
}00 
}11 †
fG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Presentacion\Models\ErrorViewModel.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Presentacion$ 0
.0 1
Models1 7
{ 
public 

class 
ErrorViewModel 
{ 
public 
string 
	RequestId 
{  !
get" %
;% &
set' *
;* +
}, -
public		 
bool		 
ShowRequestId		 !
=>		" $
!		% &
string		& ,
.		, -
IsNullOrEmpty		- :
(		: ;
	RequestId		; D
)		D E
;		E F
}

 
} ¸

XG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Presentacion\Program.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Presentacion$ 0
{ 
public 

class 
Program 
{ 
	protected 
Program 
( 
) 
{ 	
} 	
public 
static 
void 
Main 
(  
string  &
[& '
]' (
args) -
)- .
{ 	 
CreateWebHostBuilder  
(  !
args! %
)% &
.& '
Build' ,
(, -
)- .
.. /
Run/ 2
(2 3
)3 4
;4 5
} 	
public 
static 
IWebHostBuilder % 
CreateWebHostBuilder& :
(: ;
string; A
[A B
]B C
argsD H
)H I
=>J L
WebHost 
.  
CreateDefaultBuilder (
(( )
args) -
)- .
. 

UseStartup 
< 
Startup #
># $
($ %
)% &
;& '
} 
} Á
XG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Presentacion\Startup.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Presentacion$ 0
{ 
public 

class 
Startup 
{ 
public 
Startup 
( 
IConfiguration %
configuration& 3
)3 4
{ 	
Configuration 
= 
configuration )
;) *
} 	
public 
IConfiguration 
Configuration +
{, -
get. 1
;1 2
}3 4
public 
void 
ConfigureServices %
(% &
IServiceCollection& 8
services9 A
)A B
{ 	
services 
. 
	Configure 
< 
CookiePolicyOptions 2
>2 3
(3 4
options4 ;
=>< >
{ 
options 
. 
CheckConsentNeeded *
=+ ,
context- 4
=>5 7
true8 <
;< =
options   
.   !
MinimumSameSitePolicy   -
=  . /
SameSiteMode  0 <
.  < =
None  = A
;  A B
}!! 
)!! 
;!! 
services$$ 
.$$ 
AddMvc$$ 
($$ 
)$$ 
.$$ #
SetCompatibilityVersion$$ 5
($$5 6 
CompatibilityVersion$$6 J
.$$J K
Version_2_1$$K V
)$$V W
;$$W X
services&& 
.&& 

AddSignalR&& 
(&&  
)&&  !
;&&! "
}'' 	
public** 
void** 
	Configure** 
(** 
IApplicationBuilder** 1
app**2 5
,**5 6
IHostingEnvironment**7 J
env**K N
)**N O
{++ 	
if,, 
(,, 
env,, 
.,, 
IsDevelopment,, !
(,,! "
),," #
),,# $
{-- 
app.. 
... %
UseDeveloperExceptionPage.. -
(..- .
)... /
;../ 0
}// 
else00 
{11 
app22 
.22 
UseExceptionHandler22 '
(22' (
$str22( 5
)225 6
;226 7
app33 
.33 
UseHsts33 
(33 
)33 
;33 
}44 
app66 
.66 
UseHttpsRedirection66 #
(66# $
)66$ %
;66% &
app77 
.77 
UseStaticFiles77 
(77 
)77  
;77  !
app88 
.88 
UseCookiePolicy88 
(88  
)88  !
;88! "
app:: 
.:: 

UseSignalR:: 
(:: 
routes:: !
=>::" $
{;; 
routes<< 
.<< 
MapHub<< 
<<< 
NotificacionHub<< -
><<- .
(<<. /
$str<</ A
)<<A B
;<<B C
}== 
)== 
;== 
app?? 
.?? 
UseMvc?? 
(?? 
routes?? 
=>??  
{@@ 
routesAA 
.AA 
MapRouteAA 
(AA  
nameBB 
:BB 
$strBB #
,BB# $
templateCC 
:CC 
$strCC F
)CCF G
;CCG H
}DD 
)DD 
;DD 
}EE 	
}FF 
}GG §
vG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Presentacion\ViewModels\DetalleValidacionViewModel.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Presentacion$ 0
.0 1

ViewModels1 ;
{ 
public 

class &
DetalleValidacionViewModel +
{		 
} 
} ‚
lG:\Proyectos\Proteccion.TableroControl\Proteccion.TableroControl.Presentacion\ViewModels\TableroViewModel.cs
	namespace 	

Proteccion
 
. 
TableroControl #
.# $
Presentacion$ 0
.0 1

ViewModels1 ;
{ 
public		 

class		 
TableroViewModel		 !
{

 
public 
List 
< 
	Ejecucion 
> 
Ejecuciones *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
List 
< 

Validacion 
> 
Validaciones  ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public 
List 
< 
Topico 
> 
Tipos !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} 