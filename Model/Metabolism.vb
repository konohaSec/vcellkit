﻿#Region "Microsoft.VisualBasic::877225627b479fdfb8fe3492b2c7077b, Model\Metabolism.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    ' Structure Reaction
    ' 
    '     Function: converts, GetEquationString, ToString
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.TagData
Imports SMRUCC.genomics.ComponentModel.EquaionModel.DefaultTypes

''' <summary>
''' 为了实现网络节点的动态删除与增添，这个代谢过程模型应该是通用的
''' 即酶编号不应该是具体的基因编号
''' </summary>
Public Structure Reaction

    ''' <summary>
    ''' 反应过程编号
    ''' </summary>
    Public ID As String
    Public name As String

    ''' <summary>
    ''' 代谢底物编号
    ''' </summary>
    Public substrates As FactorString(Of Double)()
    ''' <summary>
    ''' 代谢产物编号
    ''' </summary>
    Public products As FactorString(Of Double)()
    ''' <summary>
    ''' 酶编号(KO编号或者EC编号)
    ''' </summary>
    Public enzyme As String()

    Public Overrides Function ToString() As String
        Return ID
    End Function

    Public Function GetEquationString() As String
        Dim substrates As CompoundSpecieReference() = converts(Me.substrates)
        Dim products As CompoundSpecieReference() = converts(Me.products)
        Dim model As New Equation With {
            .Reactants = substrates,
            .Products = products,
            .Reversible = True
        }

        Return model.ToString
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Private Shared Function converts(compounds As FactorString(Of Double)()) As CompoundSpecieReference()
        Return compounds _
            .Select(Function(c)
                        Return New CompoundSpecieReference With {
                            .ID = c.text,
                            .StoiChiometry = c.Factor
                        }
                    End Function) _
            .ToArray
    End Function

End Structure