namespace Samples.FSharp.StringTypeProvider

open System
open System.Reflection
open ProviderImplementation.ProvidedTypes
open Microsoft.FSharp.Core.CompilerServices
open Microsoft.FSharp.Quotations

[<TypeProvider>]
type StringTypeProvider(config: TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces(config)

    let ns = "Samples.StringTypeProvider"
    let thisAssembly = Assembly.GetExecutingAssembly()

    let t = ProvidedTypeDefinition(thisAssembly, ns, "StringTyped", Some typeof<obj>)    
    do this.AddNamespace(ns, [t])

    let staticParams = [ProvidedStaticParameter("value", typeof<string>)]
    do t.DefineStaticParameters(
        parameters = staticParams,
        instantiationFunction = (fun typeName paramValues ->
            match paramValues with
            | [| :? string as value |] ->
                let ty = ProvidedTypeDefinition(
                                thisAssembly,
                                ns,
                                typeName,
                                Some typeof<obj>
                            )
                ty
    
            | _ -> failwith "That wasn't supported!"
        )
    )

    let ctor = ProvidedConstructor(
        [], invokeCode = fun args -> <@@ "value" :> obj @@>)

    //ctor.AddXmlDoc "Initialise the awesomes"
    //ctor.AddXmlDocDelayed (fun () -> "Initializes a the awesomes")
    //ctor.AddXmlDocComputed (fun () -> "Initializes a the awesomes")
    //ty.AddMember ctor

    //let ctor = ProvidedConstructor([], invokeCode = fun args -> <@@ "My internal state" :> obj @@>)

   


[<assembly:TypeProviderAssembly>]
do()