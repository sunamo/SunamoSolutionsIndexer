namespace SunamoDevCode._sunamo.SunamoCSharpKeywords;

internal class CsKeywordsList
{
    internal static List<string> Modifier { get; set; } = null!;
    internal static List<string> AccessModifier { get; set; } = null!;
    internal static List<string> Statement { get; set; } = null!;
    internal static List<string> MethodParameter { get; set; } = null!;
    internal static List<string> Namespace { get; set; } = null!;
    internal static List<string> Operator { get; set; } = null!;
    internal static List<string> Access { get; set; } = null!;
    internal static List<string> Literal { get; set; } = null!;
    internal static List<string> Type { get; set; } = null!;
    internal static List<string> Contextual { get; set; } = null!;
    internal static List<string> Query { get; set; } = null!;

    private static bool initialized = false;

    internal static void Init()
    {
        if (!initialized)
        {
            Modifier = SHGetLines.GetLines(@"abstract
async
const
event
extern
new
override
partial
readonly
sealed
static
unsafe
virtual
volatile");

            AccessModifier = SHGetLines.GetLines(@"public
private
internal
protected");

            Statement = SHGetLines.GetLines(@"if
else
switch
case
do
for
foreach
in
while
break
continue
default
goto
return
yield
throw
try
catch
finally
checked
unchecked
fixed
lock");

            MethodParameter = SHGetLines.GetLines(@"params
ref
out");

            Namespace = SHGetLines.GetLines(@"using
extern alias");

            Operator = SHGetLines.GetLines(@"as
await
is
new
sizeof
typeof
stackalloc
checked
unchecked");

            Access = SHGetLines.GetLines(@"base
this");

            Literal = SHGetLines.GetLines(@"null
false
true
value
void");

            Type = SHGetLines.GetLines(@"bool
byte
char
class
decimal
double
enum
float
int
long
sbyte
short
string
struct
uint
ulong
ushort");

            Contextual = SHGetLines.GetLines(@"add
var
dynamic
global
set
value");

            Query = SHGetLines.GetLines(@"from
where
select
group
into
orderby
join
let
in
on
equals
by
ascending
descending");

            initialized = true;
        }
    }
}
