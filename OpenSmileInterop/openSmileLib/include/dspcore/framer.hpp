/*F***************************************************************************
 * 
 * openSMILE - the Munich open source Multimedia Interpretation by 
 * Large-scale Extraction toolkit
 * 
 * This file is part of openSMILE.
 * 
 * openSMILE is copyright (c) by audEERING GmbH. All rights reserved.
 * 
 * See file "COPYING" for details on usage rights and licensing terms.
 * By using, copying, editing, compiling, modifying, reading, etc. this
 * file, you agree to the licensing terms in the file COPYING.
 * If you do not agree to the licensing terms,
 * you must immediately destroy all copies of this file.
 * 
 * THIS SOFTWARE COMES "AS IS", WITH NO WARRANTIES. THIS MEANS NO EXPRESS,
 * IMPLIED OR STATUTORY WARRANTY, INCLUDING WITHOUT LIMITATION, WARRANTIES OF
 * MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE, ANY WARRANTY AGAINST
 * INTERFERENCE WITH YOUR ENJOYMENT OF THE SOFTWARE OR ANY WARRANTY OF TITLE
 * OR NON-INFRINGEMENT. THERE IS NO WARRANTY THAT THIS SOFTWARE WILL FULFILL
 * ANY OF YOUR PARTICULAR PURPOSES OR NEEDS. ALSO, YOU MUST PASS THIS
 * DISCLAIMER ON WHENEVER YOU DISTRIBUTE THE SOFTWARE OR DERIVATIVE WORKS.
 * NEITHER TUM NOR ANY CONTRIBUTOR TO THE SOFTWARE WILL BE LIABLE FOR ANY
 * DAMAGES RELATED TO THE SOFTWARE OR THIS LICENSE AGREEMENT, INCLUDING
 * DIRECT, INDIRECT, SPECIAL, CONSEQUENTIAL OR INCIDENTAL DAMAGES, TO THE
 * MAXIMUM EXTENT THE LAW PERMITS, NO MATTER WHAT LEGAL THEORY IT IS BASED ON.
 * ALSO, YOU MUST PASS THIS LIMITATION OF LIABILITY ON WHENEVER YOU DISTRIBUTE
 * THE SOFTWARE OR DERIVATIVE WORKS.
 * 
 * Main authors: Florian Eyben, Felix Weninger, 
 * 	      Martin Woellmer, Bjoern Schuller
 * 
 * Copyright (c) 2008-2013, 
 *   Institute for Human-Machine Communication,
 *   Technische Universitaet Muenchen, Germany
 * 
 * Copyright (c) 2013-2015, 
 *   audEERING UG (haftungsbeschraenkt),
 *   Gilching, Germany
 * 
 * Copyright (c) 2016,	 
 *   audEERING GmbH,
 *   Gilching Germany
 ***************************************************************************E*/


/*  openSMILE component:

dataFramer

*/


#ifndef __CFRAMER_HPP
#define __CFRAMER_HPP

#include <core/smileCommon.hpp>
#include <core/winToVecProcessor.hpp>

#define COMPONENT_DESCRIPTION_CFRAMER "This component creates frames from single dimensional input stream. It is possible to specify the frame step and frame size independently, thus allowing for overlapping frames or non continuous frames."
#define COMPONENT_NAME_CFRAMER "cFramer"

#undef class
class DLLEXPORT cFramer : public cWinToVecProcessor {
  private:

  protected:
    SMILECOMPONENT_STATIC_DECL_PR

    //virtual void fetchConfig();
    //virtual int myFinaliseInstance();
    //virtual int myTick(long long t);

    virtual int getMultiplier();
    //virtual int configureWriter(const sDmLevelConfig *c);
    //virtual int setupNamesForField(int idxi, const char*name, long nEl);
    virtual int doProcess(int i, cMatrix *row, FLOAT_DMEM*x);
    virtual int doProcess(int i, cMatrix *row, INT_DMEM*x);

  public:
    SMILECOMPONENT_STATIC_DECL
    
    cFramer(const char *_name);

    virtual ~cFramer();
};




#endif // __CFRAMER_HPP
