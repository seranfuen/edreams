/****************************************************************************
 * FrmMain: a dream diary application
 * Author: Sergio Ángel Verbo
 * Copyright © 2012-2019, Sergio Ángel Verbo
 ****************************************************************************/
/****************************************************************************
    This file is part of FrmMain.

    FrmMain is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FrmMain is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FrmMain.  If not, see <http://www.gnu.org/licenses/>.]
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using eDream.program;
using eDream.libs;
using EvilTools;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Factory;

namespace eDream
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var kernel = new StandardKernel())
            {
                kernel.Bind(x => { x.FromThisAssembly().SelectAllClasses().BindDefaultInterface(); });
                kernel.Bind<IEdreamsFactory>().ToFactory();

                Application.Run(kernel.Get<FrmMain>());
            }
        }
    }
}
