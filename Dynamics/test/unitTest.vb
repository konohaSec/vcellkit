﻿Imports Dynamics.Debugger
Imports Microsoft.VisualBasic.Data.csv
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports SMRUCC.genomics.GCModeller.ModellingEngine.Dynamics.Core

Module unitTest
    Sub Main()
        Call singleDirection()
    End Sub

    Sub singleDirection()
        Dim a As New Factor With {.ID = "a", .Value = 1000}
        Dim b As New Factor With {.ID = "b", .Value = 10000}
        Dim reaction As New Channel({New Variable(a, 1)}, {New Variable(b, 2)}) With {
            .bounds = {0, 1000},
            .ID = "a->b",
            .Forward = 300,
            .Reverse = New Controls With {.baseline = 0.05, .Activation = {New Variable(b, 1)}}
        }

        Dim machine As New Vessel() With {.Channels = {reaction}, .MassEnvironment = {a, b}}

        machine.Initialize()

        Dim snapshots As New List(Of DataSet)
        Dim flux As New List(Of DataSet)


        For i As Integer = 0 To 100000
            flux += New DataSet With {
                .ID = i,
                .Properties = machine.ContainerIterator().ToDictionary.FlatTable
            }
            snapshots += New DataSet With {
                .ID = i,
                .Properties = machine.MassEnvironment.ToDictionary(Function(m) m.ID, Function(m) m.Value)
            }
        Next

        Call snapshots.SaveTo("./single/test_mass.csv")
        Call flux.SaveTo("./single/test_flux.csv")
        Call machine.ToGraph.DoCall(AddressOf Visualizer.CreateTabularFormat).Save("./single/test_network/")
    End Sub
End Module